using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices.Membership;
using EventTrackerAdminWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventTrackerAdminWeb.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public partial class AdminController : BaseController
    {
        private readonly IMembershipServices _membershipSvc;
        private readonly IHouseHoldServices _householdSvc;

        public AdminController(IMembershipServices membershipSvc, IHouseHoldServices householdSvc)
        {
            _membershipSvc = membershipSvc;
            _householdSvc = householdSvc;
        }

        #region Member Management
        // GET: Admin
        //public ActionResult Members(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LastName" : "";
        //    ViewBag.DateSortParm = sortOrder == "MemberOf" ? "memberOf_desc" : "MemberOf";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;
        //    var pageSize = 10;
        //    var pageNumber = (page ?? 1);
        //    int pageCount;
        //    var members = _membershipSvc.GetMembers(pageNumber, pageSize);
        //    return View(members);
        //}

        [HttpGet]
        public ActionResult Members(string memberOf, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastnameSortParm = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
            ViewBag.MemberOfSortParm = sortOrder == "MemberOf" ? "memberOf_desc" : "memberOf";
            ViewBag.HouseholdSortParm = sortOrder == "MemberOf" ? "household_desc" : "household";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var pageSize = 10;
            var pageNumber = (page ?? 1);
            int pageCount;
            //var members = _services.GetMembers(pageNumber, pageSize);
            var members = _membershipSvc.GetMembersOf(new EventTracker.BusinessModel.SearchParameter { PageNumber = pageNumber, PageSize = pageSize, SortBy = sortOrder, SearchText = searchString, MemberOf = memberOf });
            return View(members);
        }

        [HttpGet]
        public ActionResult EditMember(int memberid)
        {
            var member = _membershipSvc.GetMember(memberid);

            return View(member);
        }

        [HttpPost]
        public ActionResult EditMember(Member aMember)
        {
            if (ModelState.IsValid)
            {
                if (aMember.MemberId > 0)
                {
                    _membershipSvc.UpdateMember(aMember);
                }
                else
                {
                    Mapper.CreateMap<Member, NewMember>();
                    var newMember = Mapper.Map<Member, NewMember>(aMember);
                    _membershipSvc.AddMember(newMember);
                }

                TempData["message"] = string.Format("{0} has been saved", aMember.FullName);
                return RedirectToAction("Members");
            }
            else
            {
                // there is something wrong with the data values
                return View(aMember);
            }
        }


        public ActionResult DeleteMember(int memberId)
        {
            //AT:TODO

            var anEvent = _membershipSvc.GetMember(memberId);

            if (anEvent != null & _membershipSvc.DeleteMember(memberId))
            {
                TempData["message"] = string.Format("{0} was deleted",
                    anEvent.FullName);
            }
            return RedirectToAction("Members");
        }

        public ActionResult CreateMember()
        {
            ViewBag.PanelHeading = "Add Member";
            return View("EditMember", new Member() { MemberOf = "CFC" });
        }


        public ActionResult MembershipHistory(int memberid, string memberOf)
        {
            var mHistory = _membershipSvc.GetMembershipHistory(memberid);
            //TODO: logic to determine available memberships
            var membershipOptions = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "NONE", Value = "NONE"},
                new SelectListItem { Text = "PRE-K", Value = "PRE-K"},
                new SelectListItem { Text = "KFC", Value = "KFC"},
                new SelectListItem { Text = "YFC", Value = "YFC"},
                new SelectListItem { Text = "SFC", Value = "SFC"},
                new SelectListItem { Text = "CFC", Value = "CFC"},
                new SelectListItem { Text = "HOLD", Value = "HOLD"},
                new SelectListItem { Text = "SOLD", Value = "SOLD"},
            }, "Value", "Text", memberOf);

            var mHistoryViewModel = new MembershipHistoryViewModel
            {
                MemberId = memberid,
                MemberOf = memberOf,
                MembershipHistory = mHistory,
                MembershipOptions = membershipOptions
            };
            //TempData["heading"] = 
            return View(mHistoryViewModel);
        }

        [HttpPost]
        public RedirectToRouteResult UpdateMembership(MembershipHistoryViewModel viewModel, string memberOfOldValue)
        {
            if (viewModel.MemberOf != memberOfOldValue)
            {
                //existingMember.MemberOf = memberof;
                var member = new Member() { MemberId = viewModel.MemberId, MemberOf = viewModel.MemberOf };
                _membershipSvc.UpdateMemberOf(member);
            }
            return RedirectToAction("Members","Admin");
        }

        public ActionResult AddSpouse(int spousememberid, string spouseName, string gender)
        {
            var arr = spouseName.Split(',');
            var lname = (arr.Length > 0) ? arr[0] : string.Empty;
            ViewBag.PanelHeading = string.Format("Adding Spouse Of {0}", spouseName);
            var oppositeGender = (gender.ToLower().Equals("m")) ? "F" : "M";
            return View("EditMember", new Member() { SpouseMemberId = spousememberid, SpouseName = spouseName, Gender = oppositeGender, MemberOf = "CFC", LastName= lname });
        }

        public ActionResult AddChild(int parentmemberid)
        {
            var parentMember = _membershipSvc.GetMember(parentmemberid);
            int? fatherId = null;
            int? motherId = null;
            Member father = null;
            Member mother = null;
            string parentNames;
            var lname = string.Empty;
            //TODO: Refactor
            if (parentMember.Gender == "M")
            {
                fatherId = parentMember.MemberId;
                lname = parentMember.LastName;
                //father = _membershipSvc.GetMember(fatherId.Value);
                if (parentMember.SpouseMemberId.HasValue)
                {
                    motherId = parentMember.SpouseMemberId.Value;
                    mother = _membershipSvc.GetMember(motherId.Value);
                    parentNames = $"{parentMember.FirstName}/{mother.FirstName} {parentMember.LastName}";
                }
                else
                {
                    parentNames = $"{parentMember.FirstName} {parentMember.LastName}";
                }
            }
            else
            {
                motherId = parentMember.MemberId;
                lname = parentMember.LastName;
                if (parentMember.SpouseMemberId.HasValue)
                {
                    fatherId = parentMember.SpouseMemberId.Value;
                    father = _membershipSvc.GetMember(fatherId.Value);
                    parentNames = $"{father.FirstName}/{parentMember.FirstName} {father.LastName}";
                }
                else
                {
                    parentNames = $"{parentMember.FirstName} {parentMember.LastName}";
                }
            }

            ViewBag.PanelHeading = string.Format("Adding Child Of {0}", parentNames);
            return View("EditMember", new Member() { FatherMemberId = fatherId, MotherMemberId = motherId, MemberOf = "KFC", LastName=lname  });
        } 
        #endregion


    }
}
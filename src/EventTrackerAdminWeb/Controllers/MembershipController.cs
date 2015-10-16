#region directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using EventTracker.BusinessModel.Common;
using EventTracker.BusinessModel.Criterias;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices.Membership;
using EventTrackerAdminWeb.Filter;
using PagedList;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    //[CustomAuthorize(Roles = "Admin")]
    public class MembershipController : BaseController
    {
        private readonly IMembershipServices _services;

        public MembershipController(IMembershipServices services)
        {
            _services = services;
        }

        [HttpGet]
        public ActionResult Members(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LastName" : "";
            ViewBag.DateSortParm = sortOrder == "MemberOf" ? "memberOf_desc" : "MemberOf";

            if (searchString != null) {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var pageSize = 10;
            var pageNumber = (page ?? 1);
            int pageCount;
            var members = _services.GetMembers(pageNumber, pageSize);
            return View(members);
        }



        //[HttpGet]
        //public ActionResult Households(string sortOrder, string currentFilter, string searchString, int? page) {
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LastName" : "";
        //    ViewBag.DateSortParm = sortOrder == "MemberOf" ? "memberOf_desc" : "MemberOf";

        //    if (searchString != null) {
        //        page = 1;
        //    }
        //    else {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;
        //    var pageSize = 10;
        //    var pageNumber = (page ?? 1);
        //    int pageCount;

        //    var pagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = 5 };
        //    var houseHoldCriteria = new HouseHoldCriteria();
        //    var houseHolds = _services.GetHouseHolds(houseHoldCriteria, pagingInfo);
        //    return View("HouseHolds", houseHolds);
        //}

        [HttpGet]
        public ActionResult EditMember(int memberid)
        {
            var member = _services.GetMember(memberid);

            return View(member);
        }

        [HttpPost]
        public ActionResult EditMember(Member aMember) {
            if (ModelState.IsValid) {
                if (aMember.MemberId > 0) {
                    _services.UpdateMember(aMember);
                }
                else
                {
                    Mapper.CreateMap<Member, NewMember>();
                    var newMember = Mapper.Map<Member, NewMember>(aMember);
                    _services.AddMember(newMember);
                }

                TempData["message"] = string.Format("{0} has been saved", aMember.FullName);
                return RedirectToAction("Members");
            }
            else {
                // there is something wrong with the data values
                return View(aMember);
            }
        }




        public ActionResult Delete()
        {
            //AT:TODO
            return View("Edit", new Member());
        }

        public ActionResult Create()
        {
            return View("EditMember", new Member());
        }


        public ActionResult MembershipHistory(int memberid, string memberOf)
        {
            var mHistory = _services.GetMembershipHistory(memberid);
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

            var mHistoryViewModel = new MembershipHistoryViewModel {
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
                var member = new Member() {MemberId = viewModel.MemberId, MemberOf = viewModel.MemberOf};
                _services.UpdateMemberOf(member);
            }
            return RedirectToAction("Members");
        }

        public ActionResult AddSpouse(int spousememberid, string spouseName, string gender)
        {
            ViewBag.PanelHeading = string.Format("Adding Spouse Of {0}", spouseName);
            var oppositeGender = (gender.ToLower().Equals("m")) ? "F" : "M";
            return View("EditMember", new Member() { SpouseMemberId = spousememberid , SpouseName = spouseName, Gender = oppositeGender });
        }

        public ActionResult AddChild(int parentmemberid)
        {
            var parentMember = _services.GetMember(parentmemberid);
            int? fatherId = null;
            int? motherId = null;
            Member father = null;
            Member mother = null;
            string parentNames;
            //TODO: Refactor
            if (parentMember.Gender == "M")
            {
                fatherId = parentMember.MemberId;
                //father = _services.GetMember(fatherId.Value);
                if (parentMember.SpouseMemberId.HasValue)
                {
                    motherId = parentMember.SpouseMemberId.Value;
                    mother = _services.GetMember(motherId.Value);
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
                if (parentMember.SpouseMemberId.HasValue) {
                    fatherId = parentMember.SpouseMemberId.Value;
                    father = _services.GetMember(fatherId.Value);
                    parentNames = $"{father.FirstName}/{parentMember.FirstName} {father.LastName}";
                }
                else {
                    parentNames = $"{parentMember.FirstName} {parentMember.LastName}";
                }
            }
            
            ViewBag.PanelHeading = string.Format("Adding Child Of {0}", parentNames);
            return View("EditMember", new Member() { FatherMemberId = fatherId, MotherMemberId = motherId  });
        }
    }
}
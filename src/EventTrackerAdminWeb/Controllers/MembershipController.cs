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
                new SelectListItem { Text = "KFC", Value = "KFC"},
                new SelectListItem { Text = "YFC", Value = "YFC"},
                new SelectListItem { Text = "SFC", Value = "SFC"},
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
    }
}
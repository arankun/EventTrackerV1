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

        [HttpGet]
        public ActionResult HouseholdMembers(int householdId, int houseHoldLeaderMemberId, int? page) {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            
            ViewBag.HouseHoldId = householdId;
            ViewBag.HouseHoldLeaderMemberId = houseHoldLeaderMemberId;
            var members = _services.GetHouseHoldMemers(householdId, pageNumber, pageSize);
            return PartialView("_Members", members);
        }

        [HttpGet]
        public ActionResult Households(string sortOrder, string currentFilter, string searchString, int? page) {
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

            var pagingInfo = new PagingInfo() {CurrentPage = 1, ItemsPerPage = 5};
            var houseHoldCriteria = new HouseHoldCriteria();
            var houseHolds = _services.GetHouseHolds(houseHoldCriteria, pagingInfo);
            return View("HouseHolds",houseHolds);
        }

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


        public ActionResult AddMemberToHouseHold(int houseHoldId, int houseHoldLeaderMemberId) {

            var list = _services.GetHeadOfFamilyMembers(houseHoldId, houseHoldLeaderMemberId).ToList();
            //List<SelectListItem> items = list.Select(item => new SelectListItem
            //{
            //    Text = item.FullName, Value = item.MemberId.ToString()
            //}).ToList();

            //ViewBag.HeadOfFamilyMembers = new SelectList(list, "MemberId", "FullName");
            //var items = new SelectList(list, "MemberId", "FullName").ToList();
            //items.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            var newHhMember = new NewHouseholdMember()
            {
                HouseHoldId = houseHoldId,
                HouseHoldLeaderMemberId = houseHoldLeaderMemberId,
                HeadOfFamilyMembersList = new SelectList(list, "MemberId", "FullName")
            };
            return PartialView("_AddNewHouseholdMember", newHhMember);
        }

        [HttpPost]
        public ActionResult AddMemberToHouseHold(NewHouseholdMember newhhMember) {

            if (ModelState.IsValid)
            {
                _services.AddMemberToHousehold(newhhMember);

                //TempData["message"] = string.Format("{0} has been saved", newhhMember.MemberId);
                //ViewBag.HouseHoldId = newhhMember.HouseHoldId;
                //var members = _services.GetHouseHoldMemers(newhhMember.HouseHoldId, 1, 10);
                //return PartialView("_AddNewHouseholdMember", newhhMember);

                string url = Url.Action("HouseholdMembers", "Membership", new { houseHoldId = newhhMember.HouseHoldId, houseHoldLeaderMemberId = newhhMember.HouseHoldLeaderMemberId });
                return Json(new { success = true, url = url });
            }
            return PartialView("_AddNewHouseholdMember", newhhMember);
        }

        public ActionResult Delete()
        {
            //AT:TODO
            return View("Edit", new Member());
        }

        public ActionResult Create()
        {
            return View("Edit", new Member());
        }


        public ActionResult EditHousehold(int houseHoldId)
        {
            ViewBag.HouseHoldId = houseHoldId;
            
            var hhViewModel = _services.GetHouseHoldViewModel(houseHoldId);
            ViewBag.HouseHoldLeaderMemberId = hhViewModel.HouseHold.HouseHoldLeaderMemberId;
            return View(hhViewModel);
            //return PartialView("_Ind", addresses.ToList());
        }

        [Route("DeleteHouseHold/{houseHoldId}")]
        public ActionResult DeleteHouseHold(int? houseHoldId)
        {
            if (houseHoldId == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hh = _services.GetHouseHold(houseHoldId);
            if (hh == null) {
                return HttpNotFound();
            }
            return View(hh);
        }
    }
}
using AutoMapper;
using EventTracker.BusinessModel.Common;
using EventTracker.BusinessModel.Criterias;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices.Membership;
using EventTrackerAdminWeb.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventTrackerAdminWeb.Controllers
{
    public partial class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Households(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "LastName" : "";
            ViewBag.DateSortParm = sortOrder == "MemberOf" ? "memberOf_desc" : "MemberOf";

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

            var pagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = 5 };
            var houseHoldCriteria = new HouseHoldCriteria();
            var houseHolds = _householdSvc.GetHouseHolds(houseHoldCriteria, pagingInfo);
            return View("Households", houseHolds);
        }

        public ActionResult CreateHousehold()
        {
            ViewBag.Title = "New HouseHold";
            ViewBag.PanelHeading = "New HouseHold";
            var vm = new HouseHoldDetailsViewModel() { Name = "NAME OF HOUSEHOLD" };
            var list = _householdSvc.GetHeadOfFamilyMembers();
            //var list = _services.GetHeadOfFamilyMembers(houseHoldId, houseHoldLeaderMemberId).ToList();
            vm.HeadOfFamilyMembersList = new SelectList(list, "MemberId", "FullName");
            return View("EditHousehold", vm);
        }

        [HttpGet]
        public ActionResult HouseholdMembers(int householdId, int houseHoldLeaderMemberId, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            ViewBag.HouseHoldId = householdId;
            ViewBag.HouseHoldLeaderMemberId = houseHoldLeaderMemberId;
            var members = _householdSvc.GetHouseHoldMemers(householdId, pageNumber, pageSize);
            return PartialView("_Members", members);
        }

        public ActionResult EditHousehold(int houseHoldId)
        {
            ViewBag.HouseHoldId = houseHoldId;
            ViewBag.PanelHeading = "Edit HouseHold";
            //var list = _services.GetHeadOfFamilyMembers(houseHoldId, houseHoldLeaderMemberId).ToList();
            var hhViewModel = _householdSvc.GetHouseHoldViewModel(houseHoldId);
            ViewBag.PanelHeading = "Edit HouseHold - " + hhViewModel.Name ;
            //var list = _services.GetHeadOfFamilyMembers();
            var list = _householdSvc.GetHeadOfFamilyMembers(houseHoldId, hhViewModel.HouseHoldLeaderMemberId).ToList();
            hhViewModel.HeadOfFamilyMembersList = new SelectList(list, "MemberId", "FullName");
            ViewBag.HouseHoldLeaderMemberId = hhViewModel.HouseHoldLeaderMemberId;
            return View(hhViewModel);
            //return PartialView("_Ind", addresses.ToList());
        }

        //[Route("DeleteHouseHold/{houseHoldId}")]
        public ActionResult DeleteHouseHold(int? houseHoldId)
        {
            if (houseHoldId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hh = _householdSvc.GetHouseHold(houseHoldId);
            if (hh == null)
            {
                return HttpNotFound();
            }
            return View(hh);
        }

        public ActionResult AddMemberToHouseHold(int houseHoldId, int houseHoldLeaderMemberId)
        {

            var list = _householdSvc.GetHeadOfFamilyMembers(houseHoldId, houseHoldLeaderMemberId).ToList();
            //List<SelectListItem> items = list.Select(item => new SelectListItem
            //{
            //    Text = item.FullName, Value = item.MemberId.ToString()
            //}).ToList();

            //ViewBag.HeadOfFamilyMembers = new SelectList(list, "MemberId", "FullName");
            //var items = new SelectList(list, "MemberId", "FullName").ToList();
            //items.Insert(0, (new SelectListItem { Text = "", Value = "0" }));
            ViewBag.HouseHoldId = houseHoldId;
            var newHhMember = new NewHouseholdMember()
            {
                HouseHoldId = houseHoldId,
                HouseHoldLeaderMemberId = houseHoldLeaderMemberId,
                HeadOfFamilyMembersList = new SelectList(list, "MemberId", "FullName", houseHoldLeaderMemberId)
            };
            return PartialView("_AddNewHouseholdMember", newHhMember);
        }

        [HttpPost]
        public ActionResult AddMemberToHouseHold(NewHouseholdMember newhhMember)
        {

            if (ModelState.IsValid)
            {
                _householdSvc.AddMemberToHousehold(newhhMember);

                //TempData["message"] = string.Format("{0} has been saved", newhhMember.MemberId);
                //ViewBag.HouseHoldId = newhhMember.HouseHoldId;
                //var members = _services.GetHouseHoldMemers(newhhMember.HouseHoldId, 1, 10);
                //return PartialView("_AddNewHouseholdMember", newhhMember);

                string url = Url.Action("HouseholdMembers", "HouseHold", new { houseHoldId = newhhMember.HouseHoldId, houseHoldLeaderMemberId = newhhMember.HouseHoldLeaderMemberId });
                return Json(new { success = true, url = url });
            }
            return PartialView("_AddNewHouseholdMember", newhhMember);
        }

        public ActionResult RemoveMemberFromHousehold(int memberid, int householdId)
        {
            _householdSvc.RemoveHouseholdMembersByHeadOfFamilyId(householdId, memberid);
            return RedirectToAction("EditHousehold", new { houseHoldId = householdId });
        }

        [HttpPost]
        public ActionResult EditHousehold(HouseHoldDetailsViewModel houseHoldViewModel)
        {
            if (ModelState.IsValid)
            {
                var aHousehold = new HouseHold()
                {
                    HouseHoldId = houseHoldViewModel.HouseHoldId,
                    HouseHoldLeaderMemberId = houseHoldViewModel.HouseHoldLeaderMemberId,
                    Name = houseHoldViewModel.Name,
                    Area = houseHoldViewModel.Area
                };

                if (houseHoldViewModel.HouseHoldId > 0)
                {
                    _householdSvc.UpdateHousehold(aHousehold);
                }
                else
                {
                    var newHhId = _householdSvc.CreateHousehold(aHousehold);
                    houseHoldViewModel.HouseHoldId = newHhId;
                }

                TempData["message"] = string.Format("{0} has been saved", aHousehold.Name);
                //ModelState.Clear();
                return RedirectToAction("EditHousehold", new { houseHoldId = houseHoldViewModel.HouseHoldId });
                //return View(houseHoldViewModel);
                //return View("EditHousehold", houseHoldViewModel);
            }
            else
            {
                // there is something wrong with the data values
                return View("EditHousehold", houseHoldViewModel);
            }
        }
    }
}
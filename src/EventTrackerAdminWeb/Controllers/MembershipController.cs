#region directives

using System;
using System.Web.Mvc;
using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices.Membership;
using EventTrackerAdminWeb.Filter;

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
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
            //AT:ORIG var members = _services.GetMembers(pageNumber, pageSize, out pageCount).ToPagedList(pageNumber, pageSize);
            var members = _services.GetMembers(pageNumber, pageSize);
            //var members = _services.GetMembers(pageNumber, pageSize, out pageCount);//.ToPagedList(1, 1);
            //ViewBag.PageCount = pageCount;
            //return View(students.ToPagedList(pageNumber, pageSize));
            return View(members);
        }

        [HttpGet]
        public ActionResult Edit(int memberid)
        {
            var member = _services.GetMember(memberid);

            //AT: Need to do this at service level
            //if (member.SpouseMemberId.HasValue)
            //{
            //    var spouse = _services.GetMember(member.SpouseMemberId.Value);
            //    member.Spouse = spouse;
            //}

            //var hh = _services.GetHouseHold(member.MemberId);
            //if (hh != null)
            //{
            //    member.HouseHoldId = hh.HouseHoldId;
            //    member.HouseholdName = hh.Name;
            //}

            return View(member);
        }

        [HttpPost]
        public ActionResult Edit(Member aMember) {
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
                return RedirectToAction("Index");
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
            return View("Edit", new Member());
        }

        public ActionResult EditHousehold(int? householdid)
        {
            return PartialView("_EditMemberHouseHold", new HouseHold());
        }
    }
}
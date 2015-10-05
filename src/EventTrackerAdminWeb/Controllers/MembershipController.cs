using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices;
using PagedList;
namespace EventTrackerAdminWeb.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipServices _services;

        public MembershipController(IMembershipServices services)
        {
            _services = services;
        }

        // GET: Membership
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
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            int pageCount;
            //AT:ORIG var members = _services.GetMembers(pageNumber, pageSize, out pageCount).ToPagedList(pageNumber, pageSize);
            var members = _services.GetMembers(pageNumber, pageSize);
            //var members = _services.GetMembers(pageNumber, pageSize, out pageCount);//.ToPagedList(1, 1);
            //ViewBag.PageCount = pageCount;
            //return View(students.ToPagedList(pageNumber, pageSize));
            return View(members);
        }


        public ActionResult Edit(int memberid)
        {
            var member = _services.GetMember(memberid);
            if (member.SpouseMemberId > 0)
            {
                var spouse = _services.GetMember(member.SpouseMemberId);
                member.Spouse = spouse;
            }
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
    }
}
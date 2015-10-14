#region directives

using System;
using System.Web.Mvc;
using EventTracker.BusinessModel;
using EventTracker.BusinessServices;
using EventTrackerAdminWeb.Filter;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    //[CustomAuthorize]
    public class EventsController : BaseController
    {
        private readonly IEventServices _services;

        public EventsController(IEventServices services)
        {
            _services = services;
        }

        // GET: Event
        public ActionResult Index()
        {
            var model = _services.GetAllEvents();
            return View(model);
        }

        public ActionResult Edit(int eventid)
        {
            var anEvent = _services.GetEventById(eventid);
            return View(anEvent);
        }

        [HttpPost]
        public ActionResult Edit(Event anEvent) {
            if (ModelState.IsValid)
            {
                if (anEvent.EventId > 0)
                {
                    _services.UpdateEvent(anEvent.EventId, anEvent);
                }
                else
                {
                    _services.CreateEvent(anEvent);
                }

                TempData["message"] = string.Format("{0} has been saved", anEvent.EventName);
                return RedirectToAction("Index");
            }
            else {
                // there is something wrong with the data values
                return View(anEvent);
            }
        }

        public ActionResult Delete(int eventId)
        {
            var anEvent = _services.GetEventById(eventId);

            if (anEvent != null & _services.DeleteEvent(eventId))
            {
                TempData["message"] = string.Format("{0} was deleted",
                    anEvent.EventName);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ViewBag.Title = "New Event";
            return View("Edit", new Event() { EventDate = DateTime.Now.Date});
        }
    }
}
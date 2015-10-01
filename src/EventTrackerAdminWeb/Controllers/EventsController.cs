using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventTracker.BusinessServices;

namespace EventTrackerAdminWeb.Controllers
{
    public class EventsController : Controller
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
    }
}
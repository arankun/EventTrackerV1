using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventTracker.BusinessModel;
using EventTracker.BusinessServices;

namespace EventTrackerAPI.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        private IEventServices _eventService;

        public EventController(IEventServices eventServices)
        {
            _eventService = eventServices;
        }

        public IHttpActionResult Get()
        {
            var events = _eventService.GetAllEvents();
            if (events != null)
                return Ok(events);
            else
            {
                return NotFound();
            }
        }

        public IHttpActionResult Get(int id)
        {
            var anEvent = _eventService.GetEventById(id);
            if (anEvent != null)
                return Ok(anEvent);
            else
            {
                return NotFound();
            }
        }

        public int Post([FromBody] Event newEvent)
        {
            return _eventService.CreateEvent(newEvent);
        }

        public bool Put(int id, [FromBody]Event existingEvent)
        {
            if (id > 0)
            {
                return _eventService.UpdateEvent(id, existingEvent);
            }
            return false;
        }

        // DELETE api/product/5
        public bool Delete(int id)
        {
            if (id > 0)
                return _eventService.DeleteEvent(id);
            return false;
        }
    }
}

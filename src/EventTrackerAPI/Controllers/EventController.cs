#region directives

using System.Web.Http;
using EventTracker.BusinessModel;
using EventTracker.BusinessServices;

#endregion

namespace EventTrackerAPI.Controllers
{
    //[AuthorizationRequired]
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        private readonly IEventServices _eventService;

        public EventController(IEventServices eventServices)
        {
            _eventService = eventServices;
        }

        [Route("getall")]
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

        [Route("{eventId}/attendance")]
        public IHttpActionResult GetEventAttendance(int eventId)
        {
            var attendance = _eventService.GetEventWithAttendance(eventId);
            if (attendance != null)
                return Ok(attendance);
            else
            {
                return NotFound();
            }
        }

        [Route("{eventId}/attendancesummary")]
        public IHttpActionResult GetEventAttendanceSummary(int eventId)
        {
            var attendance = _eventService.GetEventAttendanceSummary(eventId);
            if (attendance != null)
                return Ok(attendance);
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

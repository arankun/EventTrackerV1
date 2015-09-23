using System.Collections.Generic;
using EventTracker.BusinessModel;

namespace EventTracker.BusinessServices
{
    public interface IEventServices
    {
        Event GetEventById(int eventId);
        IEnumerable<Event> GetAllEvents();
        int CreateEvent(Event anEvent);
        bool UpdateEvent(int eventId, Event anEvent);
        bool DeleteEvent(int eventId);
    }
}
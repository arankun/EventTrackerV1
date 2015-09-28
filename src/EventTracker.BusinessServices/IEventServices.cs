#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel;

#endregion

namespace EventTracker.BusinessServices
{
    public interface IEventServices
    {
        Event GetEventById(int eventId);
        IEnumerable<Event> GetAllEvents();
        int CreateEvent(Event anEvent);
        bool UpdateEvent(int eventId, Event anEvent);
        bool DeleteEvent(int eventId);
        //List<DataModel.Generated.Event> GetAttendanceByEventId(int eventId);

        List<Event> GetEventWithAttendanceDetails(int eventId);
        List<EventAttendanceSummary> GetEventWithAttendanceUsingProjection(int eventId);
        List<EventAttendanceSummary> GetEventAttendanceSummary(int eventId);
    }
}
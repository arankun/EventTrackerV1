#region directives

using System.Collections.Generic;

#endregion

namespace EventTracker.BusinessModel
{
    public class Event
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public System.DateTime EventDate { get; set; }

        public virtual IEnumerable<EventAttendance> EventAttendances { get; set; }
    }
}
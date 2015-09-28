#region directives

using System;

#endregion

namespace EventTracker.BusinessModel
{
    public class EventAttendanceSummary
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }

        public string AttendeeName { get; set; }

        public int AttendeeLogCount { get; set; }
    }
}

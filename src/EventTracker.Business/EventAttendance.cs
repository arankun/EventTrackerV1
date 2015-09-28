#region directives

using System;

#endregion

namespace EventTracker.BusinessModel
{
    public class EventAttendance
    {
        public int EventAttendanceId { get; set; }
        public int UserId { get; set; }

        public Member Attendee { get; set; }
        public virtual AppUser LogByUser { get; set; }
        public int? LogBy { get; set; }

        public int EventId { get; set; }
        public DateTime LogDate { get; set; }
    }
}
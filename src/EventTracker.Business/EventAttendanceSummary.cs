using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

#region directives

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion

namespace EventTracker.BusinessModel
{
    public class Event
    {
        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [Display(Name = "Event Name")]
        [Required(ErrorMessage = "Event Name is a required field")]
        public string EventName { get; set; }

        [Display(Name = "Event Date")]
        [Required(ErrorMessage = "Event Date is a required field")]
        public System.DateTime EventDate { get; set; }

        public virtual IEnumerable<EventAttendance> EventAttendances { get; set; }
    }
}
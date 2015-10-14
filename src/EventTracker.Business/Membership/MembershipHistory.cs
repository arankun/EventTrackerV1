using System;
using System.ComponentModel.DataAnnotations;

namespace EventTracker.BusinessModel.Membership
{
    public class MembershipHistory
    {
        public int MembershipHistoryId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MemberOf { get; set; }
    }
}
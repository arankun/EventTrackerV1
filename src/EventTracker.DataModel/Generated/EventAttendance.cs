//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventTracker.DataModel.Generated
{
    using System;
    using System.Collections.Generic;
    
    public partial class EventAttendance
    {
        public int EventAttendanceId { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public Nullable<int> LogBy { get; set; }
        public System.DateTime LogDate { get; set; }
        public string IsRegistered { get; set; }
        public Nullable<int> RegisteredBy { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
    
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual Member Member { get; set; }
    }
}

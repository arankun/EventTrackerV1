using System.Collections.Generic;
using System.Web.Mvc;

namespace EventTracker.BusinessModel.Membership
{
    public class HouseHoldDetailsViewModel
    {
        //public HouseHold HouseHold { get; set; }
        public IEnumerable<Member> HouseHoldMembers { get; set; }
        public int HouseHoldId { get; set; }
        public int HouseHoldLeaderMemberId { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string State { get; set; }
        public string HouseHoldLeader { get; set; }
        public SelectList HeadOfFamilyMembersList { get; set; }
    }
}
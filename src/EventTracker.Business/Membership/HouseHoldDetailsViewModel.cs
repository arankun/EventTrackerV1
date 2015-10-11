using System.Collections.Generic;

namespace EventTracker.BusinessModel.Membership
{
    public class HouseHoldDetailsViewModel
    {
        public HouseHold HouseHold { get; set; }
        public IEnumerable<Member> HouseHoldMembers { get; set; }
    }
}
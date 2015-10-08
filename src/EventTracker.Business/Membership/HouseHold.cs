namespace EventTracker.BusinessModel.Membership
{
    public class HouseHold
    {
         public int HouseHoldId { get; set; }
         public int HouseHoldLeaderMemberId { get; set; }
         public string Name { get; set; }
        public string Area { get; set; }
        public string State { get; set; }
        public string HouseHoldLeader { get; set; }
    }
}
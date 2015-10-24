namespace EventTracker.BusinessModel.Membership
{
    public class NewMember:BaseMember
    {
        public int? SpouseMemberId { get; set; }
        public int? FatherMemberId { get; set; }
        public int? MotherMemberId { get; set; }
        public bool IsHeadOfFamily { get; set; }
    }
}

namespace EventTracker.BusinessModel
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Name { get { return LastName + ", " + FirstName.Substring(0,1); }  }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
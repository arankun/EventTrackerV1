#region directives

using System;

#endregion

namespace EventTracker.BusinessModel.Membership
{
    public abstract class BaseMember
    {
        public string Name { get { return LastName + ", " + FirstName.Substring(0,1); }  }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
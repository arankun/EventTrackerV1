#region directives

using System.Security.Principal;

#endregion

namespace EventTrackerAPI.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string userName, string password)
            : base(userName, "Basic")
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public int UserId { get; set; }
    }
}
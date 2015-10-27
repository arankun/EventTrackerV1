#region directives

using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

#endregion

namespace EventTracker.BusinessModel.Authentication
{
    public class CustomPrincipal:IPrincipal
    {
        public CustomPrincipal(string username) {
            Identity = new GenericIdentity(username);
        }

        public CustomPrincipal(string username, string[] roles)
        {
            Identity = new GenericIdentity(username);
            Roles = roles.ToList();
        }

        public int UserId { get; set; }
        public List<string> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (Roles.Any(role.Contains)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
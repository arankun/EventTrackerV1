#region directives

using System.Collections.Generic;

#endregion

namespace EventTracker.BusinessModel.Authentication {

    public class CustomPrincipalSerializeModel {
        public int UserId { get; set; }
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}

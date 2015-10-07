using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTracker.BusinessServices.Authentication {

    public class SecuredAppUser {
        public SecuredAppUser() {
            FeaturesAndRestrictedFieldsForUser = new Dictionary<string, List<string>>();
        }

        public int UserId { get; set; }
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DisplayName { get; set; }

        public List<string> Roles { get; set; }
        public List<string> AvailableFeatures { get; set; }
        public Dictionary<string, List<string>> FeaturesAndRestrictedFieldsForUser { get; set; }
    }
}

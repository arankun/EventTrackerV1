using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EventTracker.BusinessModel.Membership {
    public class NewHouseholdMember {
        public int HouseHoldId { get; set; }

        [DisplayName("Member/Family")]
        public int MemberId { get; set; }
        public string FamilyName { get; set; }
        public SelectList HeadOfFamilyMembersList { get; set; }
    }
}

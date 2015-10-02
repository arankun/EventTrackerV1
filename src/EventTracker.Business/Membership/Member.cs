using System.ComponentModel;
using System.Web.Mvc;

namespace EventTracker.BusinessModel.Membership
{
    public class Member:BaseMember
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberId { get; set; }
        public string MemberOf { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SpouseMemberId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ParentMemberId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Member Spouse { get; set; }
    }
}
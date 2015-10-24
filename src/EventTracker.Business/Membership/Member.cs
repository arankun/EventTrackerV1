#region directives

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace EventTracker.BusinessModel.Membership
{
    public class Member:BaseMember
    {
        [HiddenInput(DisplayValue = false)]
        public int MemberId { get; set; }

        public string MemberOf { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? SpouseMemberId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string SpouseName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ParentMemberId { get; set; }

        //public string Household { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? HouseHoldId { get; set; }

        public string HouseholdName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? FatherMemberId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? MotherMemberId { get; set; }

        [Display(Name = "Is Head of Family", Order = 7)]
        public bool IsHeadOfFamily { get; set; }
    }
}
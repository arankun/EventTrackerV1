#region directives

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
#endregion

namespace EventTracker.BusinessModel.Membership
{
    public abstract class BaseMember
    {
        public string Name { get { return LastName + ", " + FirstName.Substring(0,1); }  }

        public string FullName { get { return LastName + ", " + FirstName; } }

        [Display(Name = "Last Name", Order = 1)]
        [Required(ErrorMessage = "Last Name is a required field")]
        public string LastName { get; set; }

        [Display(Name = "First Name", Order = 2)]
        public string FirstName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date of Birth", Order = 3)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Order = 4)]
        public string Gender { get; set; }

        [Display(Order = 5)]
        public string Phone { get; set; }

        [Display(Order = 6)]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MemberOf { get; set; }
    }
}
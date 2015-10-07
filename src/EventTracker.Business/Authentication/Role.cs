#region directives

using System.ComponentModel.DataAnnotations;

#endregion

namespace EventTracker.BusinessModel.Authentication
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
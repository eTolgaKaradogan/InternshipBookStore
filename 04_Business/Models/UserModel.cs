using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class UserModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsBlocked { get; set; }

        [DisplayName("Is Blocked?")]
        public string IsBlockedText { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "{0} is required!")]
        public int RoleId { get; set; }

        public RoleModel Role { get; set; }
    }
}

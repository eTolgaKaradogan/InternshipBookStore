using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class PasswordRecoveryModel : RecordBase
    {
        [DisplayName("E-mail")]
        [EmailAddress]
        [Required(ErrorMessage = "{0} is required!")]
        public string Email { get; set; }

        [DisplayName("Recovery Code")]
        [Required(ErrorMessage = "{0} is required!")]
        public string RecoveryCode { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Password { get; set; }

    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class ForgotPasswordModel : RecordBase
    {
        [DisplayName("E-mail")]
        [EmailAddress]
        [Required(ErrorMessage = "{0} is required!")]
        public string Email { get; set; }
    }
}

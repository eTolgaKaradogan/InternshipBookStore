using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class CategoryModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [DisplayName("Book Count")]
        public int BookCount { get; set; }
    }
}

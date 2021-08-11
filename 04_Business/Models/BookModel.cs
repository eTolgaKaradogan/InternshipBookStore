using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;
using _02_Entities.Entities;

namespace _04_Business.Models
{
    public class BookModel : RecordBase
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(2, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "{0} is required!")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "{0} is required!")]
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")]
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 9999, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int StockAmount { get; set; }

        [DisplayName("Author")]
        [Required(ErrorMessage = "{0} is required!")]
        public string AuthorName { get; set; }

        [DisplayName("ISBN")]
        [Required(ErrorMessage = "{0} is required!")]
        public long Isbn { get; set; }

        [DisplayName("Is Enabled?")]
        [Required(ErrorMessage = "{0} is required!")]
        public bool IsEnabled { get; set; } = true;

        public List<Review> Reviews { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        [StringLength(255, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Image")]
        public string ImageFileName { get; set; }
    }
}

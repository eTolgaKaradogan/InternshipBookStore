using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Book : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public int StockAmount { get; set; }

        [Required]
        [StringLength(70)]
        public string AuthorName { get; set; }

        public long Isbn { get; set; }

        public List<Review> Reviews { get; set; }

        [StringLength(255)]
        public string ImageFileName { get; set; }

        public bool IsEnabled { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

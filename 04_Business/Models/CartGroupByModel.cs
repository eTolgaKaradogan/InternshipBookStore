using System;
using System.ComponentModel;

namespace _04_Business.Models
{
    public class CartGroupByModel
    {
        public int BookId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [DisplayName("Total Unit Price")]
        public string TotalUnitPriceText { get; set; }

        [DisplayName("Total Count")]
        public int TotalCount { get; set; }
    }
}

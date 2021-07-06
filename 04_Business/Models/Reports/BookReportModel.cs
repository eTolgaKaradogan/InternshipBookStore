using System;
using System.ComponentModel;

namespace _04_Business.Models.Reports
{
    public class BookReportModel
    {
        [DisplayName("Book Name")]
        public string BookName { get; set; }

        public string BookDescription { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Author")]
        public string AuthorName { get; set; }

        public long Isbn { get; set; }

        public double Rate { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPriceText { get; set; }

        [DisplayName("Stock Amount")]
        public int StockAmount { get; set; }

        public int CategoryId { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsEnabled { get; set; }
    }
}

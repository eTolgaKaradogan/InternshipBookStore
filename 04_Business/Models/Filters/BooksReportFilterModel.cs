using System;
using System.ComponentModel;
using _01_AppCore.Business.Validations;

namespace _04_Business.Models.Filters
{
    public class BooksReportFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Book Name")]
        public string BookName { get; set; }

        [DisplayName("Author")]
        public string AuthorName { get; set; }

        [DisplayName("Unit Price")]
        [StringDecimal]
        public string UnitPriceBeginText { get; set; }

        [StringDecimal]
        public string UnitPriceEndText { get; set; }

        [DisplayName("Stock Amount")]
        public int? StockAmountBegin { get; set; }

        public int? StockAmountEnd { get; set; }

        [DisplayName("ISBN")]
        public long Isbn { get; set; }
    }
}

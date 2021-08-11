using System;
using _02_Entities.Entities;
using _04_Business.Models;

namespace _05_MvcWebUI.Models
{
    public class BookDetailsViewModel
    {
        public BookModel bookModel { get; set; }

        public Review Review { get; set; }
    }
}

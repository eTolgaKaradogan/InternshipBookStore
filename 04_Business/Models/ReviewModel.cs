using System;
using System.Collections.Generic;
using _01_AppCore.Records.Bases;
using _02_Entities.Entities;

namespace _04_Business.Models
{
    public class ReviewModel : RecordBase
    {
        public double Rating { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public int BookId { get; set; }
        public BookModel Book { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}

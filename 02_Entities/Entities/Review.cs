using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Review : RecordBase
    {
        [StringLength(1000, ErrorMessage = "The maximum is 1000 characters.")]
        public string Content { get; set; }

        public string Rating { get; set; }

        public string Username { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

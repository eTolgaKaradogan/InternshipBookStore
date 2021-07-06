using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Category : RecordBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Book> Books { get; set; }
    }
}

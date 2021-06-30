using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Role : RecordBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<User> Users { get; set; }

    }
}

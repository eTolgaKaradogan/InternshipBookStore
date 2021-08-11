using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class User : RecordBase
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [Required]
        [StringLength(70)]
        public string Email { get; set; }

        public bool IsBlocked { get; set; }

        public string PasswordResetCode { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Watchlist> Watchlists { get; set; }

        public List<NotificationUser> NotificationUsers { get; set; }

    }
}

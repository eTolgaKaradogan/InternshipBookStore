using System;
using System.ComponentModel;
using _01_AppCore.Records.Bases;
using _02_Entities.Entities;

namespace _04_Business.Models
{
    public class WatchlistModel : RecordBase
    {
        public int UserId { get; set; }

        public UserModel User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int followedUserId { get; set; }

        public string currentUsername { get; set; }

        [DisplayName("Username")]
        public string followedUsername { get; set; }


    }
}

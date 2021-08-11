using System;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Watchlist : RecordBase
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int followedUserId { get; set; }

        public string followedUsername { get; set; }
    }
}
using System;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class NotificationUser : RecordBase
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool IsRead { get; set; } = false;


    }
}

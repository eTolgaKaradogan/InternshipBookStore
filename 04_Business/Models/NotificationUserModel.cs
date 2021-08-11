using System;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class NotificationUserModel : RecordBase
    {
        public int NotificationId { get; set; }
        public NotificationModel Notification { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}

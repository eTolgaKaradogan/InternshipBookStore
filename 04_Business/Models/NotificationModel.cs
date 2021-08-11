using System;
using System.Collections.Generic;
using _01_AppCore.Records.Bases;

namespace _04_Business.Models
{
    public class NotificationModel : RecordBase
    {
        public string Text { get; set; }
        public bool IsRead { get; set; } = false;

        public List<NotificationUserModel> NotificationUserModels { get; set; }
    }
}

using System;
using System.Collections.Generic;
using _01_AppCore.Records.Bases;

namespace _02_Entities.Entities
{
    public class Notification : RecordBase
    {
        public string Text { get; set; }

        public List<NotificationUser> NotificationUsers { get; set; }
    }
}

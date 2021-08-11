using System;
using System.Collections.Generic;
using _01_AppCore.Business.Services.Bases;
using _02_Entities.Entities;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface INotificationService : IService<NotificationModel>
    {
        List<NotificationUser> GetUserNotifications(int id);
        void Create(Notification notification, string username);
        void ReadNotification(int notificationId, int userId);
    }
}

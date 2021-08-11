using System;
using System.Linq;
using System.Security.Claims;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _05_MvcWebUI.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly NotificationRepositoryBase _notificationRepository;

        public NotificationController(INotificationService notificationService, IUserService userService, NotificationRepositoryBase notificationRepository)
        {
            _notificationService = notificationService;
            _userService = userService;
            _notificationRepository = notificationRepository;
        }

        public IActionResult GetNotification()
        {
            int userId = _userService.GetUserId(User.Identity.Name);
            var notification = _notificationService.GetUserNotifications(userId);
            return Ok(new { UserNotification = notification, Count = notification.Count });
        }

        public IActionResult ReadNotification(int notificationId)
        {
            int userId = _userService.GetUserId(User.Identity.Name);
            _notificationService.ReadNotification(notificationId, userId);
            return Ok();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace _04_Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepositoryBase _notificationRepository;
        private readonly NotificationUserRepositoryBase _notificationUserRepository;
        private readonly IWatchlistService _watchlistService;

        public NotificationService(NotificationRepositoryBase notificationRepository, IWatchlistService watchlistService, NotificationUserRepositoryBase notificationUserRepository)
        {
            _notificationRepository = notificationRepository;
            _watchlistService = watchlistService;
            _notificationUserRepository = notificationUserRepository;
        }

        public Result Add(NotificationModel model)
        {
            try
            {
                var entity = new Notification()
                {
                    Text = model.Text,
                };
                _notificationRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex);
            }
        }

        public void Create(Notification notification, string username)
        {
            try
            {
                _notificationRepository.Add(notification);

                var watchlists = _watchlistService.GetWatchlistByUsername(username);
                foreach (var watchlist in watchlists)
                {
                    var userNotification = new NotificationUser();
                    userNotification.UserId = watchlist.UserId;
                    userNotification.NotificationId = notification.Id;
                    _notificationUserRepository.Add(userNotification);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _notificationRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _notificationRepository?.Dispose();
        }

        public List<NotificationUser> GetUserNotifications(int id)
        {
            return _notificationUserRepository.Query().Where(u => u.UserId == id && !u.IsRead).Include(n => n.Notification).ToList();
        }

        public IQueryable<NotificationModel> Query()
        {
            var query = Query().Select(c => new NotificationModel()
            {
                Text = c.Text,
                IsRead = c.IsRead,
                NotificationUserModels = c.NotificationUserModels
            });
            return query;
        }

        public void ReadNotification(int notificationId, int userId)
        {
            var notification = _notificationUserRepository.Query().FirstOrDefault(n => n.UserId == userId && n.NotificationId == notificationId);
            notification.IsRead = true;
            _notificationUserRepository.Update(notification);
        }

        public Result Update(NotificationModel model)
        {
            try
            {
                var entity = Query().SingleOrDefault(a => a.Id == model.Id);
                entity.Id = model.Id;
                entity.IsRead = model.IsRead;
                entity.Text = model.Text;
                Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}

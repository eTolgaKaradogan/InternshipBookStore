using System;
using System.Collections.Generic;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly WatchlistRepositoryBase _watchlistRepository;
        private readonly UserRepositoryBase _userRepository;

        public WatchlistService(WatchlistRepositoryBase watchlistRepository, UserRepositoryBase userRepository)
        {
            _watchlistRepository = watchlistRepository;
            _userRepository = userRepository;
        }

        public Result Add(WatchlistModel model)
        {
            try
            {
                var entity = new Watchlist()
                {
                    UserId = model.UserId,
                    CreatedDate = model.CreatedDate,
                    followedUserId = model.followedUserId,
                    followedUsername = model.followedUsername
                };
                _watchlistRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckIfAlreadyExists(int followedUserId, int currentUserId)
        {
            return _watchlistRepository.Query().Any(w => w.followedUserId == followedUserId && w.UserId == currentUserId);
        }

        public Result Delete(int id)
        {
            try
            {
                _watchlistRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _watchlistRepository?.Dispose();
        }

        public List<WatchlistModel> GetUserWatchlist(int id)
        {
            var user = new User();
            var query = _watchlistRepository.Query().Select(c => new WatchlistModel()
            {
                Id = c.Id,
                UserId = c.UserId,
                followedUserId = c.followedUserId,
                CreatedDate = c.CreatedDate,
                followedUsername = c.followedUsername
            }).Where(w => w.UserId == id).ToList();
            return query;
        }

        public Watchlist GetWatchlist(int id)
        {
            return _watchlistRepository.Query().FirstOrDefault(w => w.Id == id);
        }

        public List<WatchlistModel> GetWatchlistByUsername(string username)
        {
            var followedUser = _userRepository.Query().FirstOrDefault(u => u.UserName == username);
            return Query().Where(w => w.followedUserId == followedUser.Id).ToList();
        }

        public IQueryable<WatchlistModel> Query()
        {
            var query = _watchlistRepository.Query().Select(c => new WatchlistModel()
            {
                UserId = c.UserId,
                CreatedDate = c.CreatedDate,
                followedUserId = c.followedUserId
            });
            return query;
        }

        public void Remove(Watchlist watchlist)
        {
            try
            {
                _watchlistRepository.Delete(watchlist);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Result Update(WatchlistModel model)
        {
            try
            {
                var entity = _watchlistRepository.Query().SingleOrDefault(a => a.Id == model.Id);
                entity.Id = model.Id;
                entity.UserId = model.UserId;
                entity.CreatedDate = model.CreatedDate;
                entity.followedUserId = model.followedUserId;
                _watchlistRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}

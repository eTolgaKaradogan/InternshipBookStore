using System;
using System.Collections.Generic;
using _01_AppCore.Business.Services.Bases;
using _02_Entities.Entities;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface IWatchlistService : IService<WatchlistModel>
    {
        Watchlist GetWatchlist(int id);
        List<WatchlistModel> GetUserWatchlist(int id);
        void Remove(Watchlist watchlist);
        bool CheckIfAlreadyExists(int followedUserId, int currentUserId);

        List<WatchlistModel> GetWatchlistByUsername(string username);
    }
}

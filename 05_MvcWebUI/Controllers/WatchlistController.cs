﻿using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _05_MvcWebUI.Controllers
{
    [Authorize]
    public class WatchlistController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWatchlistService _watchlistService;
        private readonly WatchlistRepositoryBase _watchlistRepository;
        private readonly UserRepositoryBase _userRepository;

        public WatchlistController(IUserService userService, IWatchlistService watchlistService, UserRepositoryBase userRepository, WatchlistRepositoryBase watchlistRepository)
        {
            _userService = userService;
            _watchlistService = watchlistService;
            _userRepository = userRepository;
            _watchlistRepository = watchlistRepository;
        }

        public IActionResult Index(string currentUsername)
        {
            var currentUser = _userService.Query().FirstOrDefault(u => u.UserName == currentUsername && u.IsBlocked == false);
            var watchlist = _watchlistService.GetUserWatchlist(currentUser.Id);
            return View(watchlist);
        }

        public IActionResult Add(string username, string currentUsername)
        {
            var followedUser = _userService.Query().SingleOrDefault(u => u.UserName == username);
            var currentUser = _userService.Query().SingleOrDefault(u => u.UserName == currentUsername);
            var watchlist = new WatchlistModel()
            {
                UserId = currentUser.Id,
                followedUserId = followedUser.Id,
                followedUsername = followedUser.UserName
            };
            bool exists = _watchlistService.CheckIfAlreadyExists(followedUser.Id, currentUser.Id);
            if (exists == true)
            {
                Notify("The user is already following.");
                return View();
            }
            var result = _watchlistService.Add(watchlist);
            if (result.Status == ResultStatus.Success)
            {
                Notify("{0} is added to your watchlist.", followedUser.UserName);
                return RedirectToAction("Index", "Watchlist", new { currentUsername = currentUser.UserName });
            }
            Notify("An error occured!");
            return View("Error");
        }


        public IActionResult Search(string searchString)
        {
            ViewData["CurrentSearch"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                var watchUser = _userService.Query().SingleOrDefault(b => b.UserName == searchString && b.IsBlocked == false);
                if (watchUser == null)
                {
                    Notify("There is no user with this username.");
                    return View();
                }
                return View(watchUser);
            }

            return View(ResultStatus.Error);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                Notify("An error occured!");
                return View("NotFound");
            }
            var result = _watchlistService.Delete(id.Value);
            if (result.Status == ResultStatus.Exception)
            {
                Notify("An error occured!");
            }
            Notify("User is deleted from your watchlist.");
            return RedirectToAction("Index");
        }
    }
}

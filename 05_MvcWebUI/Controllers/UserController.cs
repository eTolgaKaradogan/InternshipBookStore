using System;
using System.Linq;
using System.Threading.Tasks;
using _01_AppCore.Business.Models.Results;
using _04_Business.Enums;
using _04_Business.Models;
using _04_Business.Services.Bases;
using _05_MvcWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _05_MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var model = _userService.Query();

            return View(await PaginatedList<UserModel>.CreateAsync(model, pageNumber, 6));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _userService.GetUser(id.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                ViewData["Message"] = result.Message;
            return View(result.Data);
        }

        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_roleService.Query().ToList(), "Id", "Name", (int)Roles.Admin);
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var userResult = _userService.Add(user);
                if (userResult.Status == ResultStatus.Exception)
                    Notify("An error occured!");
                if (userResult.Status == ResultStatus.Success)
                {
                    Notify("User is created.");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", userResult.Message);
            }
            ViewData["Roles"] = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
            return View(user);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var userResult = _userService.GetUser(id.Value);
            if (userResult.Status == ResultStatus.Exception)
                throw new Exception(userResult.Message);
            if (userResult.Status == ResultStatus.Error)
                return View("NotFound");
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", userResult.Data.RoleId);
            return View(userResult.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var userResult = _userService.Update(user);
                if (userResult.Status == ResultStatus.Exception)
                    Notify("An error occured!");
                if (userResult.Status == ResultStatus.Success)
                {
                    Notify("User is successfully edited.");
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", userResult.Message);
            }
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
            return View(user);
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            _userService.Delete(id.Value);
            Notify("User is deleted.");
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult FollowUser(string searchString, string currentUserName)
        {
            ViewData["CurrentSearch"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                var followingUser = _userService.Query().SingleOrDefault(b => b.UserName == searchString);
                if (followingUser == null)
                {
                    Notify("The error occured!");
                    return View(ResultStatus.Error);
                }
                var user = _userService.Query().SingleOrDefault(u => u.UserName == currentUserName);
                user.FollowingUsers = followingUser.UserName;
                _userService.Update(user);
                Notify($"You have followed {followingUser.UserName}.");
                return RedirectToAction("Index", "Book");
            }

            return View(ResultStatus.Error);
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _04_Business.Enums;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _05_MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var result = _userService.GetUsers();
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                ViewBag.Message = result.Message;
            return View(result.Data);
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

        // GET: Users/Create
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
                    throw new Exception(userResult.Message);
                if (userResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
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
                    throw new Exception(userResult.Message);
                if (userResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", userResult.Message);
            }
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
            return View(user);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _userService.Delete(id.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}

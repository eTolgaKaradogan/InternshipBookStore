using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _04_Business.Enums;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _05_MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var model = _roleService.Query().ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new RoleModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Add(role);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }
                throw new Exception(result.Message);
            }
            return View(role);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            var role = _roleService.Query().SingleOrDefault(r => r.Id == id.Value);
            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(role);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }

                throw new Exception(result.Message);
            }
            return View(role);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var deleteResult = _roleService.Delete(id);
            if (deleteResult.Status == ResultStatus.Exception)
                deleteResult.Message = "An exception occured while deleting the role!";
            return Json(deleteResult.Message);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            var role = _roleService.Query().SingleOrDefault(r => r.Id == id.Value);
            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }
    }
}

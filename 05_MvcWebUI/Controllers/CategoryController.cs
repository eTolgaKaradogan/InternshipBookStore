using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _05_MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var model = _categoryService.Query().ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new CategoryModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Add(category);
                if (result.Status == ResultStatus.Success)
                {
                    Notify("Category is created!");
                    return RedirectToAction(nameof(Index));
                }
                if (result.Status == ResultStatus.Error)
                {
                    Notify("An error occured!");
                    return View(category);
                }
                throw new Exception(result.Message);
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }
            var category = _categoryService.Query().SingleOrDefault(c => c.Id == id.Value);
            if (category == null)
            {
                return View("NotFound");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(category);
                if (result.Status == ResultStatus.Success)
                {
                    Notify("Category is successfuly edited!");
                    return RedirectToAction(nameof(Index));
                }

                if (result.Status == ResultStatus.Error)
                {
                    Notify("An error occured!");
                    return View(category);
                }
                throw new Exception(result.Message);
            }
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            var model = _categoryService.Query().SingleOrDefault(c => c.Id == id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleteResult = _categoryService.Delete(id);
            if (deleteResult.Status == ResultStatus.Success)
            {
                Notify("Category is successfuly deleted!");
                return RedirectToAction(nameof(Index));
            }

            if (deleteResult.Status == ResultStatus.Error)
            {
                Notify("An error occured!");
                var category = _categoryService.Query().SingleOrDefault(c => c.Id == id);
                return View("Edit", category);
            }
            throw new Exception(deleteResult.Message);
        }
    }
}

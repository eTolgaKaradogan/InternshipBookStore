using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _01_AppCore.Business.Models.Results;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace _05_MvcWebUI.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke(int? categoryId)
        {
            List<CategoryModel> categories;
            Task<Result<List<CategoryModel>>> task = _categoryService.GetCategoriesAsync(); // *3
            Result<List<CategoryModel>> result = task.Result;
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            categories = result.Data;

            ViewBag.CategoryId = categoryId;
            return View(categories);
        }
    }
}

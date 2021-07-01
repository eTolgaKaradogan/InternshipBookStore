using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _01_AppCore.Business.Models.Results;
using _01_AppCore.Business.Services.Bases;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface ICategoryService : IService<CategoryModel>
    {
        Task<Result<List<CategoryModel>>> GetCategoriesAsync();
    }
}

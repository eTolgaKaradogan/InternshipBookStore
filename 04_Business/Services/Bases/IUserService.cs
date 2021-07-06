using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using _01_AppCore.Business.Models.Results;
using _01_AppCore.Business.Services.Bases;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface IUserService : IService<UserModel>
    {
        Result<List<UserModel>> GetUsers();
        Result<UserModel> GetUser(int id);
        Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate);
    }
}

using System;
using _01_AppCore.Business.Models.Results;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface IAccountService
    {
        Result Register(UserRegisterModel model);
        Result<UserModel> Login(UserLoginModel model);
    }
}

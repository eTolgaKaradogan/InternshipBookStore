using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _01_AppCore.Business.Services.Bases;
using _04_Business.Models;

namespace _04_Business.Services.Bases
{
    public interface IAccountService : IService<AccountModel>
    {
        Result Register(UserRegisterModel model);
        Result<UserModel> Login(UserLoginModel model);

    }
}

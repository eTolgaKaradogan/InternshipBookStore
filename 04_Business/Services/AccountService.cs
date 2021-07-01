using System;
using _01_AppCore.Business.Models.Results;
using _04_Business.Enums;
using _04_Business.Models;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result<UserModel> Login(UserLoginModel model)
        {
            try
            {
                return _userService.GetUser(u => u.UserName == model.UserName && u.Password == model.Password && !u.IsBlocked);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result Register(UserRegisterModel model)
        {
            try
            {
                var user = new UserModel()
                {
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    Email = model.Email.Trim(),
                    RoleId = (int)Roles.User,
                    IsBlocked = false
                };
                return _userService.Add(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}

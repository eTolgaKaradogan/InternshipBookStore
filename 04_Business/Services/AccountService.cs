using System;
using System.Linq;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Enums;
using _04_Business.Models;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly UserRepositoryBase _userRepository;

        public AccountService(IUserService userService, UserRepositoryBase userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        public Result Add(AccountModel model)
        {
            var entity = new User()
            {
                UserName = model.UserName.Trim(),
                Password = model.Password.Trim(),
                Email = model.Email.Trim(),
                IsBlocked = model.IsBlocked,
                RoleId = model.RoleId
            };
            _userRepository.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            try
            {
                var account = _userRepository.Query().SingleOrDefault(a => a.Id == id);
                _userRepository.Delete(account);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _userRepository?.Dispose();
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

        public IQueryable<AccountModel> Query()
        {
            var query = _userRepository.Query().Select(c => new AccountModel()
            {
                Id = c.Id,
                UserName = c.UserName,
                Password = c.Password,
                Email = c.Email,
                IsBlocked = c.IsBlocked,
                RoleId = c.RoleId,
                PasswordResetCode = c.PasswordResetCode
            });
            return query;
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

        public Result Update(AccountModel model)
        {
            try
            {
                var entity = _userRepository.Query().SingleOrDefault(a => a.Id == model.Id);
                entity.Id = model.Id;
                entity.UserName = model.UserName;
                entity.Password = model.Password;
                entity.Email = model.Email;
                entity.RoleId = model.RoleId;
                entity.IsBlocked = model.IsBlocked;
                entity.PasswordResetCode = model.PasswordResetCode;
                _userRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}

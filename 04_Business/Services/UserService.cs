using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using _01_AppCore.Business.Models.Results;
using _02_Entities.Entities;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;

namespace _04_Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepositoryBase _userRepository;

        public UserService(UserRepositoryBase userRepository)
        {
            _userRepository = userRepository;
        }

        public Result Add(UserModel model)
        {
            try
            {
                if (_userRepository.EntityQuery().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.EntityQuery().Any(u => u.Email.ToUpper() == model.Email.ToUpper().Trim()))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    IsBlocked = model.IsBlocked,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    Email = model.Email.Trim(),
                    RoleId = model.RoleId
                };
                _userRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _userRepository.DeleteEntity(id);
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

        public Result<UserModel> GetUser(int id)
        {
            try
            {
                var user = Query().SingleOrDefault(u => u.Id == id);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate)
        {
            try
            {
                var user = Query().SingleOrDefault(predicate);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result<List<UserModel>> GetUsers()
        {
            try
            {
                var users = Query().ToList();
                if (users == null || users.Count == 0)
                    return new ErrorResult<List<UserModel>>("No users found!");
                return new SuccessResult<List<UserModel>>(users);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<UserModel>>(exc);
            }
        }

        public IQueryable<UserModel> Query()
        {
            var userQuery = _userRepository.EntityQuery("Role");

            var query = from user in userQuery
                        select new UserModel()
                        {
                            IsBlocked = user.IsBlocked,
                            IsBlockedText = user.IsBlocked ? "Yes" : "No",
                            Id = user.Id,
                            Password = user.Password,
                            Email = user.Email,
                            Role = new RoleModel()
                            {
                                Id = user.Role.Id,
                                Name = user.Role.Name
                            },
                            RoleId = user.RoleId,
                            UserName = user.UserName
                        };
            return query;
        }

        public Result Update(UserModel model)
        {
            try
            {
                if (_userRepository.EntityQuery().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim() && u.Id != model.Id))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.EntityQuery().Any(u => u.Email.ToUpper() == model.Email.ToUpper().Trim()))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    Id = model.Id,
                    IsBlocked = model.IsBlocked,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    Email = model.Email.Trim(),
                    RoleId = model.RoleId,
                };
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using _01_AppCore.Business.Models.Results;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Models;
using _04_Business.Services.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

namespace _05_MvcWebUI.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly UserRepositoryBase _userRepository;
        private readonly IUserService _userService;

        public AccountController(IAccountService accountService, UserRepositoryBase userRepository, IUserService userService)
        {
            _accountService = accountService;
            _userRepository = userRepository;
            _userService = userService;
        }

        public IActionResult Register()
        {
            var model = new UserRegisterModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Register(model);
                if (result.Status == ResultStatus.Exception)
                {
                    Notify("An error occured!");
                    throw new Exception(result.Message);
                }
                if (result.Status == ResultStatus.Success)
                {
                    Notify("You have successfully registered!");
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public IActionResult Login()
        {
            var model = new UserLoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if (result.Status == ResultStatus.Exception)
                {
                    Notify("An error occured!");
                    throw new Exception(result.Message);
                }

                if (result.Status == ResultStatus.Error)
                {
                    Notify("An error occured!");
                    return View(model);
                }

                var user = result.Data;

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string username)
        {
            var user = _accountService.Query().FirstOrDefault(u => u.UserName == username);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AccountModel user)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Update(user);

                if (result.Status == ResultStatus.Success)
                {
                    Logout();
                    Notify("You have successfully edited your profile!");
                    return RedirectToAction("Login");
                }

                if (result.Status == ResultStatus.Error)
                {
                    Notify("An error occured!");
                    return View(user);
                }
                throw new Exception(result.Message);
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var user = _accountService.Query().FirstOrDefault(u => u.UserName == username);

            return View(user);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email, string[] args)
        {
            try
            {

                var user = _accountService.Query().FirstOrDefault(u => u.Email == email);

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                user.PasswordResetCode = finalString;
                _accountService.Update(user);

                var eMail = new MimeMessage();
                eMail.From.Add(MailboxAddress.Parse("tolgamvcproject@gmail.com"));
                eMail.To.Add(MailboxAddress.Parse(user.Email));
                eMail.Subject = "Password Recovery";
                eMail.Body = new TextPart(TextFormat.Html) { Text = "<h1>There is password recovery code in the below:</h1>" + finalString };

                using (var smtp = new SmtpClient())
                {
                    ;
                    smtp.Connect("smtp.gmail.com", 587, false);
                    smtp.Authenticate("tolgamvcproject@gmail.com", "mvcprojectmail");
                    smtp.Send(eMail);
                    smtp.Disconnect(true);
                }
                Notify("Password recovery is sent to your email.");
                return RedirectToAction("PasswordRecovery");

            }
            catch (Exception ex)
            {
                Notify("An error occured!");
                throw ex;
            }
        }

        [HttpGet]
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordRecovery(string email, string password, string recoveryCode)
        {
            var user = _accountService.Query().FirstOrDefault(u => u.Email == email);

            if (user.PasswordResetCode == recoveryCode)
            {
                user.Password = password;
                _accountService.Update(user);
                Notify("You have successfully changed your password.");
                return RedirectToAction("Login");
            }
            else
            {
                Notify("An error occured!");
                return View();
            }
        }
    }
}
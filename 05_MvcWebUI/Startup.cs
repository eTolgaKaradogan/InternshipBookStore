using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using _01_AppCore.Business.Utils;
using _01_AppCore.Business.Utils.Bases;
using _02_Entities.Entities;
using _03_DataAccess.EntityFramework.Contexts;
using _03_DataAccess.Repositories;
using _03_DataAccess.Repositories.Bases;
using _04_Business.Services;
using _04_Business.Services.Bases;
using _05_MvcWebUI.Controllers;
using _05_MvcWebUI.Models;
using _05_MvcWebUI.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System.IO;

namespace _05_MvcWebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.LoginPath = "/Accounts/Login";
                    config.AccessDeniedPath = "/Accounts/AccessDenied";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    config.SlidingExpiration = true;
                });

            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromMinutes(30);
            });


            services.AddScoped<DbContext, Context>();
            services.AddScoped<BookRepositoryBase, BookRepository>();
            services.AddScoped<CategoryRepositoryBase, CategoryRepository>();
            services.AddScoped<UserRepositoryBase, UserRepository>();
            services.AddScoped<RoleRepositoryBase, RoleRepository>();
            services.AddScoped<ReviewRepositoryBase, ReviewRepository>();
            services.AddScoped<WatchlistRepositoryBase, WatchlistRepository>();
            services.AddScoped<NotificationRepositoryBase, NotificationRepository>();
            services.AddScoped<NotificationUserRepositoryBase, NotificationUserRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IWatchlistService, WatchlistService>();

            AppSettingsUtilBase appSettingsUtil = new AppSettingsUtil(Configuration);
            appSettingsUtil.Bind<AppSettings>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

//TODO******* dotnet ef database update - permission denied hatası veriyor. database update edilecek.
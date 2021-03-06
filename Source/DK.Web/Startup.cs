﻿using DK.Application.Models;
using DK.Application.Repositories;
using DK.Web.DependencyResolution;
using DK.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using MongoDB.AspNet.Identity;
using MongoDB.Driver;
using Owin;

[assembly: OwinStartupAttribute(typeof(DK.Web.Startup))]
namespace DK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            InitAdmin();
        }

        private static void InitAdmin()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(IoC.Container.GetInstance<IMongoDatabase>()));
            var username = "admin";
            var admin = userManager.FindByName(username);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = username,
                    Email = $"{username}@email.com",
                    LastName = "Quản trị hệ thống",
                    Status = 1,
                };
                userManager.Create(admin, "1");
            }

            if (!userManager.IsInRole(admin.Id, RoleList.SupperAdmin))
            {
                userManager.AddToRole(admin.Id, RoleList.SupperAdmin);
            }
            var typeRepo = IoC.Container.GetInstance<ITypeRepository>();
            var type = typeRepo.Get(TypeConstant.TaiSanSequenceId);
            if (type == null) typeRepo.Add(new Type { Id = TypeConstant.TaiSanSequenceId, Number = 0 });
        }
    }
}

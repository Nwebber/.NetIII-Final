using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MVCMovieRentalFinal.Models;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(MVCMovieRentalFinal.Startup))]
namespace MVCMovieRentalFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "scott@webber.com";
                user.Email = "scott@webber.com";
                string userPassword = "newuser";
                var checkUser = userManager.Create(user, userPassword);

                if (checkUser.Succeeded)
                {
                    var resultAdmin = userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("IT"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "IT";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}

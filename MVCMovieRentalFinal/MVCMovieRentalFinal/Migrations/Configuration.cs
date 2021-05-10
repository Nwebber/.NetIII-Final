namespace MVCMovieRentalFinal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCMovieRentalFinal.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCMovieRentalFinal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCMovieRentalFinal.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string admin = "admin@admin.com";
            const string defaultPassword = "newuser";

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "IT" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "User" });

            context.SaveChanges();

            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges();

                if (result.Succeeded)
                {
                    // we created the admin, now give the admin the Admin role
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }
            }
        }
    }
}

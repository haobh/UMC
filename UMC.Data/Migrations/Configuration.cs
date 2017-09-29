namespace UMC.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UMCDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UMCDbContext context)
        {
            CreateUser(context);
        }
        private void CreateUser(UMCDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new UMCDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new UMCDbContext()));
            var user = new ApplicationUser()
            {
                UserName = "haobh",
                Email = "haobh88@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "Bui Huu Hao"

            };
            manager.Create(user, "123");
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }
            var adminUser = manager.FindByEmail("haobh88@gmail.com");
            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }
    }
}

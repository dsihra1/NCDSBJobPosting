namespace CommunityProject_ProjectUnity.DAL.SecurityMigrations
{
    using CommunityProject_ProjectUnity.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommunityProject_ProjectUnity.DAL.SecurityMigrations.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\SecurityMigrations";
        }

        protected override void Seed(CommunityProject_ProjectUnity.DAL.SecurityMigrations.ApplicationDbContext context)
        {
            //Create a Role Manager
            var roleManager = new RoleManager<IdentityRole>(new
                                          RoleStore<IdentityRole>(context));
            //Create Role Admin if it does not exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Admin"));
            }
            //Create Role Supervisor if it does not exist
            if (!context.Roles.Any(r => r.Name == "Human Resources"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Human Resources"));
            }
            //Create Role Security if it does not exist
            if (!context.Roles.Any(r => r.Name == "Security"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Security"));
            }


            //Create a User Manager
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //-----------------------------------------
            //Now the Admin user named admin1 with password password
            var adminuser = new ApplicationUser
            {
                UserName = "admin1@outlook.com",
                Email = "admin1@outlook.com"
            };

            //Assign admin user to role
            if (!context.Users.Any(u => u.UserName == "admin1@outlook.com"))
            {
                manager.Create(adminuser, "password");
                manager.AddToRole(adminuser.Id, "Admin");
                manager.AddToRole(adminuser.Id, "Security");
            }
            //-----------------------------------------------------
            //Now the Security user named security1 with password password
            var securityuser = new ApplicationUser
            {
                UserName = "security1@outlook.com",
                Email = "security1@outlook.com"
            };

            //Assign security user to role
            if (!context.Users.Any(u => u.UserName == "security1@outlook.com"))
            {
                manager.Create(securityuser, "password");
                manager.AddToRole(securityuser.Id, "Security");
            }
            //-----------------------------------------------------
            //Now the Supervisor user named supervisor1 with password password
            var supervisoruser = new ApplicationUser
            {
                UserName = "humanresources1@outlook.com",
                Email = "humanresources1@outlook.com"
            };

            //Assign supervisor user to role
            if (!context.Users.Any(u => u.UserName == "humanresources1@outlook.com"))
            {
                manager.Create(supervisoruser, "password");
                manager.AddToRole(supervisoruser.Id, "Human Resources");
            }
            //-----------------------------------------------------
            //Create a few generic users
            for (int i = 1; i <= 4; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = string.Format("user{0}@outlook.com", i.ToString()),
                    Email = string.Format("user{0}@outlook.com", i.ToString())
                };
                if (!context.Users.Any(u => u.UserName == user.UserName))
                    manager.Create(user, "password");
            }
        }
    }
}


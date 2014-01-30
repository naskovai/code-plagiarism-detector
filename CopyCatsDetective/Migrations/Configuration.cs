namespace CopyCatsDetective.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CopyCatsDetective.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<CopyCatsDetective.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CopyCatsDetective.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string name = "admin";
            string password = "123456";

            //Create User=Admin with password=123456
            var user = new ApplicationUser();
            user.UserName = name;
            var adminresult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (adminresult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, name);
            }
            base.Seed(context);
        }

        private void EnsureRolesInitialized(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExists(Roles.Admin))
            {
                roleManager.Create(new IdentityRole(Roles.Admin));
            }

            if (!roleManager.RoleExists(Roles.Member))
            {
                roleManager.Create(new IdentityRole(Roles.Member));
            }
        }
    }
}

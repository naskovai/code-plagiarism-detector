using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CopyCatsDetective.Models
{
    public static class Roles
    {
        public const string Admin = "admin";
        public const string Member = "member";
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Organization> Organizations { get; set; }

        public System.Data.Entity.DbSet<CopyCatsDetective.Models.Category> Categories { get; set; }
    }
}
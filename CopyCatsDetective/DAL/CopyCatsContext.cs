using CopyCatsDetective.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CopyCatsDetective.DAL
{
    public class CopyCatsContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Coder> Coders { get; set; }
        public DbSet<SourceCode> SourceCodes { get; set; }
        public DbSet<CodePool> CodePools { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
    }
}
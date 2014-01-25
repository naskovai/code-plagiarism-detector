namespace CopyCatsDetective.Migrations
{
    using CopyCatsDetective.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CopyCatsDetective.DAL.CopyCatsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CopyCatsDetective.DAL.CopyCatsContext context)
        {
            List<Account> accounts = new List<Account>()
            {
                new Account { Name = "Slav Petkov", Password = "1234"},
                new Account { Name = "Milen Petkov", Password = "0000"},
                new Account { Name = "Ivan Petkov", Password = "1111"},
                new Account { Name = "Milen Ivanov", Password = "2222"},
            };

            accounts.ForEach
                (
                    account => context.Accounts.AddOrUpdate(account)
                );
            context.SaveChanges();

            List<Coder> coders = new List<Coder>()
            {
                new Coder { Name = "Slav Petkov", Email = "slav_p@abv.bg", SourceCodes = new List<SourceCode>()},
                new Coder { Name = "Milen Ivanov", Email = "milen_i@abv.bg", SourceCodes = new List<SourceCode>()},
                new Coder { Name = "Kris Naidenov", Email = "kris_n@abv.bg", SourceCodes = new List<SourceCode>()},
                new Coder { Name = "Teodor Blajev", Email = "teodor_b@abv.bg", SourceCodes = new List<SourceCode>()},
            };

            coders.ForEach
                (
                    coder => context.Coders.AddOrUpdate(coder)
                );
            context.SaveChanges();

            List<SourceCode> sourceCodes = new List<SourceCode>()
            {
                new SourceCode { Language = "C#", ActualCode = "Console.Write('Hello');", Coder = new Coder { Name = "Milen"}},
                //new SourceCode { Language = "C#", ActualCode = "Console.WriteLine('Hello');"},
                //new SourceCode { Language = "C#", ActualCode = "Console.ReadLine();"},
                //new SourceCode { Language = "C#", ActualCode = "Console.Read();"},
            };

            sourceCodes.ForEach
                (
                    sourceCode => context.SourceCodes.AddOrUpdate(sourceCode)
                );
            context.SaveChanges();
        }
    }
}

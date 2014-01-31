namespace CopyCatsDetective.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentProfile", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StudentProfile", new[] { "User_Id" });
            AddColumn("dbo.StudentProfile", "UserId", c => c.String());
            DropColumn("dbo.StudentProfile", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentProfile", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.StudentProfile", "UserId");
            CreateIndex("dbo.StudentProfile", "User_Id");
            AddForeignKey("dbo.StudentProfile", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

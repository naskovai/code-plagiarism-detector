namespace CopyCatsDetective.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cotegories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Category", "ParentCategory_Id", "dbo.Category");
            DropIndex("dbo.Category", new[] { "ParentCategory_Id" });
            RenameColumn(table: "dbo.Category", name: "ParentCategory_Id", newName: "ParentCategoryId");
            AlterColumn("dbo.Category", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Category", "ParentCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Category", "ParentCategoryId");
            AddForeignKey("dbo.Category", "ParentCategoryId", "dbo.Category", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Category", "ParentCategoryId", "dbo.Category");
            DropIndex("dbo.Category", new[] { "ParentCategoryId" });
            AlterColumn("dbo.Category", "ParentCategoryId", c => c.Int());
            AlterColumn("dbo.Category", "Id", c => c.Int(nullable: false, identity: true));
            RenameColumn(table: "dbo.Category", name: "ParentCategoryId", newName: "ParentCategory_Id");
            CreateIndex("dbo.Category", "ParentCategory_Id");
            AddForeignKey("dbo.Category", "ParentCategory_Id", "dbo.Category", "Id");
        }
    }
}

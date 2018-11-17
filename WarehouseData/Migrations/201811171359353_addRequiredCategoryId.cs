namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredCategoryId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "CategoryId", "dbo.Category");
            DropIndex("dbo.Product", new[] { "CategoryId" });
            AlterColumn("dbo.Product", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Product", "CategoryId");
            AddForeignKey("dbo.Product", "CategoryId", "dbo.Category", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product", "CategoryId", "dbo.Category");
            DropIndex("dbo.Product", new[] { "CategoryId" });
            AlterColumn("dbo.Product", "CategoryId", c => c.Int());
            CreateIndex("dbo.Product", "CategoryId");
            AddForeignKey("dbo.Product", "CategoryId", "dbo.Category", "Id");
        }
    }
}

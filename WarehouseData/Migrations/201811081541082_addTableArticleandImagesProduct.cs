namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableArticleandImagesProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Content = c.String(),
                        DateCreated = c.DateTime(),
                        OrderNum = c.Int(),
                        Display = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImagesProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Image = c.String(maxLength: 310),
                        OrderNum = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImagesProducts", "ProductId", "dbo.Product");
            DropIndex("dbo.ImagesProducts", new[] { "ProductId" });
            DropTable("dbo.ImagesProducts");
            DropTable("dbo.Articles");
        }
    }
}

namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableProductTranslation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductTranslation",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 500),
                        Content = c.String(),
                        Alias_SEO = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.LanguageId })
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Use = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductTranslation", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductTranslation", "LanguageId", "dbo.Language");
            DropIndex("dbo.ProductTranslation", new[] { "LanguageId" });
            DropIndex("dbo.ProductTranslation", new[] { "ProductId" });
            DropTable("dbo.Language");
            DropTable("dbo.ProductTranslation");
        }
    }
}

namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableCategoryTranslation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryTranslation",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias_SEO = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.LanguageId })
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryTranslation", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.CategoryTranslation", "CategoryId", "dbo.Category");
            DropIndex("dbo.CategoryTranslation", new[] { "LanguageId" });
            DropIndex("dbo.CategoryTranslation", new[] { "CategoryId" });
            DropTable("dbo.CategoryTranslation");
        }
    }
}

namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableBlogTranslation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogTranslation",
                c => new
                    {
                        BlogId = c.Int(nullable: false),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Content = c.String(nullable: false),
                        Alias = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.BlogId, t.LanguageId })
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.BlogId)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogTranslation", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.BlogTranslation", "BlogId", "dbo.Blogs");
            DropIndex("dbo.BlogTranslation", new[] { "LanguageId" });
            DropIndex("dbo.BlogTranslation", new[] { "BlogId" });
            DropTable("dbo.BlogTranslation");
        }
    }
}

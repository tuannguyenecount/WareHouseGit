namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtableArticleTranslation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleTranslation",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 256),
                        Content = c.String(),
                        Alias = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.LanguageId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleTranslation", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.ArticleTranslation", "ArticleId", "dbo.Articles");
            DropIndex("dbo.ArticleTranslation", new[] { "LanguageId" });
            DropIndex("dbo.ArticleTranslation", new[] { "ArticleId" });
            DropTable("dbo.ArticleTranslation");
        }
    }
}

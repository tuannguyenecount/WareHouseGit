namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlisttableTranslation3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlideTranslation",
                c => new
                    {
                        SlideId = c.Int(nullable: false),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => new { t.SlideId, t.LanguageId })
                .ForeignKey("dbo.Language", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Slide", t => t.SlideId, cascadeDelete: true)
                .Index(t => t.SlideId)
                .Index(t => t.LanguageId);
            
        }
        
        public override void Down()
        {
           
            DropForeignKey("dbo.SlideTranslation", "SlideId", "dbo.Slide");
            DropForeignKey("dbo.SlideTranslation", "LanguageId", "dbo.Language");
            DropIndex("dbo.SlideTranslation", new[] { "LanguageId" });
            DropIndex("dbo.SlideTranslation", new[] { "SlideId" });
            DropTable("dbo.SlideTranslation");

        }
    }
}

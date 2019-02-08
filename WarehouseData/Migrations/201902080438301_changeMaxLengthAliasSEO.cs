namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeMaxLengthAliasSEO : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Alias_SEO", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(maxLength: 300, unicode: false));
            AlterColumn("dbo.CategoryTranslation", "Alias_SEO", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.ProductTranslation", "Alias_SEO", c => c.String(maxLength: 300, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductTranslation", "Alias_SEO", c => c.String(nullable: false, maxLength: 256, unicode: false));
            AlterColumn("dbo.CategoryTranslation", "Alias_SEO", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(maxLength: 256, unicode: false));
            AlterColumn("dbo.Product", "Alias_SEO", c => c.String(nullable: false, maxLength: 256, unicode: false));
        }
    }
}

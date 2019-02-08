namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredAliasProduct : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Product", "Alias_SEO", c => c.String(maxLength: 256));
            //AlterColumn("dbo.ProductTranslation", "Alias_SEO", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", "UNIQUR_Product_Alias_SEO");
            //AlterColumn("dbo.ProductTranslation", "Alias_SEO", c => c.String(nullable: false, maxLength: 256));
            //AlterColumn("dbo.Product", "Alias_SEO", c => c.String(nullable: false, maxLength: 256));
        }
    }
}

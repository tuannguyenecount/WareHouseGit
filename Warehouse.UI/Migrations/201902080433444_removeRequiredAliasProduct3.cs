namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredAliasProduct3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Alias_SEO", c => c.String(maxLength: 300));
            AlterColumn("dbo.ProductTranslation", "Alias_SEO", c => c.String(maxLength: 300));
        }

        public override void Down()
        {
        }
    }
}

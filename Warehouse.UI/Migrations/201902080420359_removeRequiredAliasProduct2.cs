namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredAliasProduct2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Product", "UNIQUR_Product_Alias_SEO");
        }

        public override void Down()
        {
            
        }
    }
}

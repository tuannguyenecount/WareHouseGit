namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterColumnPricePriceNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "Price", c => c.Int(nullable: false));
            AlterColumn("dbo.Product", "PriceNew", c => c.Int(nullable: true));

        }

        public override void Down()
        {
        }
    }
}

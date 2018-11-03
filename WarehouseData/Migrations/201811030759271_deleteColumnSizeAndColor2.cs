namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnSizeAndColor2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "Color");
            DropColumn("dbo.Product", "Size");
        }
        
        public override void Down()
        {
        }
    }
}

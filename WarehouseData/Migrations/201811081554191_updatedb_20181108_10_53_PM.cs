namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb_20181108_10_53_PM : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "Slider");
            DropColumn("dbo.InfoShop", "Introduce_Shop");
            DropColumn("dbo.InfoShop", "Contact_Info");
            DropColumn("dbo.InfoShop", "TextFooter");
            DropColumn("dbo.InfoShop", "SalesPolicy");
            DropColumn("dbo.InfoShop", "ShoppingGuide");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InfoShop", "ShoppingGuide", c => c.String());
            AddColumn("dbo.InfoShop", "SalesPolicy", c => c.String());
            AddColumn("dbo.InfoShop", "TextFooter", c => c.String());
            AddColumn("dbo.InfoShop", "Contact_Info", c => c.String());
            AddColumn("dbo.InfoShop", "Introduce_Shop", c => c.String());
            AddColumn("dbo.Product", "Slider", c => c.String(maxLength: 100, unicode: false));
        }
    }
}

namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateInfoShop : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InfoShop", "ShopName", c => c.String());
            AlterColumn("dbo.InfoShop", "Logo", c => c.String(unicode: false));
            AlterColumn("dbo.InfoShop", "Phone", c => c.String(unicode: false));
            AlterColumn("dbo.InfoShop", "Zalo", c => c.String(unicode: false));
            AlterColumn("dbo.InfoShop", "Fanpage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InfoShop", "Fanpage", c => c.String(maxLength: 256));
            AlterColumn("dbo.InfoShop", "Zalo", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InfoShop", "Phone", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.InfoShop", "Logo", c => c.String(maxLength: 256, unicode: false));
            AlterColumn("dbo.InfoShop", "ShopName", c => c.String(nullable: false, maxLength: 256));
        }
    }
}

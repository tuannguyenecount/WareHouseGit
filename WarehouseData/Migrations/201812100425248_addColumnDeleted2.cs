namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnDeleted2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Order", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.OrderDetail", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Product", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Category", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "Deleted");
            DropColumn("dbo.Product", "Deleted");
            DropColumn("dbo.OrderDetail", "Deleted");
            DropColumn("dbo.Order", "Deleted");
            DropColumn("dbo.Blogs", "Deleted");
        }
    }
}

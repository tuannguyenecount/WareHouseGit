namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "Status", c => c.Byte(nullable: false));
            AlterColumn("dbo.Order", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Order", "Phone", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Order", "Address", c => c.String());
            DropColumn("dbo.Order", "Assigned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "Assigned", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Order", "Address", c => c.String(maxLength: 500));
            AlterColumn("dbo.Order", "Phone", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Order", "Name", c => c.String(nullable: false, maxLength: 300));
            DropColumn("dbo.Order", "Status");
        }
    }
}

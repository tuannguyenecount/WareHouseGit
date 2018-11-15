namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnUserUpdated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Product", "UserUpdated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "UserUpdated", c => c.String(maxLength: 256));
        }
    }
}

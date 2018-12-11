namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnDeleted4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slide", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slide", "Deleted");
        }
    }
}

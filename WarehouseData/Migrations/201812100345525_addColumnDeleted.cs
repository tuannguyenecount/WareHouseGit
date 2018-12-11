namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnDeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Deleted", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Deleted");
        }
    }
}

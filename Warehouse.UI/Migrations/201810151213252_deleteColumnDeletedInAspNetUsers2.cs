namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnDeletedInAspNetUsers2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Deleted");
        }
        
        public override void Down()
        {
        }
    }
}

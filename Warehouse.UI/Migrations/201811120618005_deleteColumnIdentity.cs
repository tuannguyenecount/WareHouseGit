namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnIdentity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
        
        public override void Down()
        {
           
        }
    }
}

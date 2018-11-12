namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnIdentity2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DateRegister", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "Phone");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DateRegister", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}

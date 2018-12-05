namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterColumnDateSubscriber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Subscriber", "DateSubscriber", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subscriber", "DateSubscriber", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}

namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetableCategory3 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Category", "ParentId", "dbo.Category", "Id");
        }
        
        public override void Down()
        {
        }
    }
}

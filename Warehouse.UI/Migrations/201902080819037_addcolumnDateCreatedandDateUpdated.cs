namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolumnDateCreatedandDateUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTranslation", "DateCreated", c => c.DateTime());
            AddColumn("dbo.ProductTranslation", "DateUpdated", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTranslation", "DateUpdated");
            DropColumn("dbo.ProductTranslation", "DateCreated");
        }
    }
}

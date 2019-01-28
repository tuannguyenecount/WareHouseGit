namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnSortOrderToTableLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Language", "SortOrder", c => c.Byte());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Language", "SortOrder");
        }
    }
}

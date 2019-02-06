namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetableCategory2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
        }
    }
}

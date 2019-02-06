namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetableCategory3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(maxLength: 256, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(nullable: false, maxLength: 256, unicode: false));
        }
    }
}

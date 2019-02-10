namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredBlog2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "Alias", c => c.String(maxLength: 300));
            AlterColumn("dbo.BlogTranslation", "Alias", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogTranslation", "Alias", c => c.String(maxLength: 256));
            AlterColumn("dbo.Blogs", "Alias", c => c.String(nullable: false));
        }
    }
}

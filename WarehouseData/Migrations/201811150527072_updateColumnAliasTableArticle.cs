namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateColumnAliasTableArticle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "Alias", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
        }
    }
}

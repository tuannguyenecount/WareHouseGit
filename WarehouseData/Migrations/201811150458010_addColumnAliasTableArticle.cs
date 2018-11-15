namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnAliasTableArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Alias", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
        }
    }
}

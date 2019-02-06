namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetableCategory1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Category", "FK_Category_Category");
            //DropIndex("dbo.Category", "UNI_Category_Alias_SEO");
            AlterColumn("dbo.Category", "Alias_SEO", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
        }
    }
}

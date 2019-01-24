namespace Warehouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addlisttableTranslation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "AspNetUser_Id", "dbo.AspNetUsers1");
            DropIndex("dbo.News", new[] { "AspNetUser_Id" });       
            DropTable("dbo.News");
        }
        
        public override void Down()
        {

        }
    }
}

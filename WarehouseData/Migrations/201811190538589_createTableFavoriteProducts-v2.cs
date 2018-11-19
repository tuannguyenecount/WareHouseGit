namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableFavoriteProductsv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FavoriteProducts", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FavoriteProducts", new[] { "AspNetUser_Id" });
            RenameColumn(table: "dbo.FavoriteProducts", name: "AspNetUser_Id", newName: "AspNetUserId");
            DropPrimaryKey("dbo.FavoriteProducts");
            AlterColumn("dbo.FavoriteProducts", "AspNetUserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.FavoriteProducts", new[] { "ProductId", "AspNetUserId" });
            CreateIndex("dbo.FavoriteProducts", "AspNetUserId");
            AddForeignKey("dbo.FavoriteProducts", "AspNetUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.FavoriteProducts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FavoriteProducts", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.FavoriteProducts", "AspNetUserId", "dbo.AspNetUsers");
            DropIndex("dbo.FavoriteProducts", new[] { "AspNetUserId" });
            DropPrimaryKey("dbo.FavoriteProducts");
            AlterColumn("dbo.FavoriteProducts", "AspNetUserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.FavoriteProducts", new[] { "ProductId", "UserId" });
            RenameColumn(table: "dbo.FavoriteProducts", name: "AspNetUserId", newName: "AspNetUser_Id");
            CreateIndex("dbo.FavoriteProducts", "AspNetUser_Id");
            AddForeignKey("dbo.FavoriteProducts", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}

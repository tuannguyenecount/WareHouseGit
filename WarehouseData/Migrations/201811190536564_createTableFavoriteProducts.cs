namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableFavoriteProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FavoriteProducts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        FavoriteDate = c.DateTime(),
                        AspNetUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProductId, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.AspNetUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.AspNetUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavoriteProducts", "ProductId", "dbo.Product");
            DropForeignKey("dbo.FavoriteProducts", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FavoriteProducts", new[] { "AspNetUser_Id" });
            DropIndex("dbo.FavoriteProducts", new[] { "ProductId" });
            DropTable("dbo.FavoriteProducts");
        }
    }
}

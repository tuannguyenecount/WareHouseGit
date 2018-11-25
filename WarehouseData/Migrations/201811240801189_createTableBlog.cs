namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableBlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Image = c.String(),
                        Content = c.String(nullable: false),
                        DateCreated = c.DateTime(),
                        Display = c.Boolean(),
                        Alias = c.String(nullable: false, maxLength: 256),
                        ViewCount = c.Int(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        UserName = c.String(maxLength: 256),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Blogs", new[] { "User_Id" });
            DropTable("dbo.Blogs");
        }
    }
}

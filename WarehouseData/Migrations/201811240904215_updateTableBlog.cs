namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTableBlog : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogs", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Blogs", name: "IX_User_Id", newName: "IX_UserId");
            DropColumn("dbo.Blogs", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "UserName", c => c.String(maxLength: 256));
            RenameIndex(table: "dbo.Blogs", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Blogs", name: "UserId", newName: "User_Id");
        }
    }
}

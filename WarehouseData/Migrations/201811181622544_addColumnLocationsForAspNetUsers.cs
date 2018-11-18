namespace Warehouse.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnLocationsForAspNetUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProvinceId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DistrictId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "WardId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ProvinceId");
            CreateIndex("dbo.AspNetUsers", "DistrictId");
            CreateIndex("dbo.AspNetUsers", "WardId");
            AddForeignKey("dbo.AspNetUsers", "DistrictId", "dbo.District", "Id");
            AddForeignKey("dbo.AspNetUsers", "ProvinceId", "dbo.Province", "Id");
            AddForeignKey("dbo.AspNetUsers", "WardId", "dbo.Ward", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "WardId", "dbo.Ward");
            DropForeignKey("dbo.AspNetUsers", "ProvinceId", "dbo.Province");
            DropForeignKey("dbo.AspNetUsers", "DistrictId", "dbo.District");
            DropIndex("dbo.AspNetUsers", new[] { "WardId" });
            DropIndex("dbo.AspNetUsers", new[] { "DistrictId" });
            DropIndex("dbo.AspNetUsers", new[] { "ProvinceId" });
            DropColumn("dbo.AspNetUsers", "WardId");
            DropColumn("dbo.AspNetUsers", "DistrictId");
            DropColumn("dbo.AspNetUsers", "ProvinceId");
        }
    }
}

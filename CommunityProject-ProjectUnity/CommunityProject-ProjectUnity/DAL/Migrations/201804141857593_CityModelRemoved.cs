namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CityModelRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Position", "City_ID", "dbo.City");
            DropForeignKey("dbo.CityPosting", "City_ID", "dbo.City");
            DropForeignKey("dbo.CityPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.School", "cityID", "dbo.City");
            DropIndex("dbo.Position", new[] { "City_ID" });
            DropIndex("dbo.School", new[] { "cityID" });
            DropIndex("dbo.CityPosting", new[] { "City_ID" });
            DropIndex("dbo.CityPosting", new[] { "Posting_ID" });
            DropColumn("dbo.Position", "City_ID");
            DropTable("dbo.City");
            DropTable("dbo.CityPosting");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CityPosting",
                c => new
                    {
                        City_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.City_ID, t.Posting_ID });
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        schoolID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Position", "City_ID", c => c.Int());
            CreateIndex("dbo.CityPosting", "Posting_ID");
            CreateIndex("dbo.CityPosting", "City_ID");
            CreateIndex("dbo.School", "cityID");
            CreateIndex("dbo.Position", "City_ID");
            AddForeignKey("dbo.School", "cityID", "dbo.City", "ID");
            AddForeignKey("dbo.CityPosting", "Posting_ID", "dbo.Posting", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CityPosting", "City_ID", "dbo.City", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Position", "City_ID", "dbo.City", "ID");
        }
    }
}

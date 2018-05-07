namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolCityModelAnotherChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Posting", "CityID", c => c.Int(nullable: false));
            AddColumn("dbo.Posting", "SchoolID", c => c.Int(nullable: false));
            AddColumn("dbo.Position", "CityID", c => c.Int(nullable: false));
            AddColumn("dbo.Position", "SchoolID", c => c.Int(nullable: false));
            CreateIndex("dbo.Posting", "CityID");
            CreateIndex("dbo.Posting", "SchoolID");
            CreateIndex("dbo.Position", "CityID");
            CreateIndex("dbo.Position", "SchoolID");
            AddForeignKey("dbo.Position", "CityID", "dbo.City", "ID");
            AddForeignKey("dbo.Position", "SchoolID", "dbo.School", "ID");
            AddForeignKey("dbo.Posting", "SchoolID", "dbo.School", "ID");
            AddForeignKey("dbo.Posting", "CityID", "dbo.City", "ID");
            DropColumn("dbo.Posting", "City");
            DropColumn("dbo.Posting", "SchoolName");
            DropColumn("dbo.Position", "PositionCity");
            DropColumn("dbo.Position", "PositionSchoolName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Position", "PositionSchoolName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Position", "PositionCity", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Posting", "SchoolName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Posting", "City", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.Posting", "CityID", "dbo.City");
            DropForeignKey("dbo.Posting", "SchoolID", "dbo.School");
            DropForeignKey("dbo.Position", "SchoolID", "dbo.School");
            DropForeignKey("dbo.Position", "CityID", "dbo.City");
            DropIndex("dbo.Position", new[] { "SchoolID" });
            DropIndex("dbo.Position", new[] { "CityID" });
            DropIndex("dbo.Posting", new[] { "SchoolID" });
            DropIndex("dbo.Posting", new[] { "CityID" });
            DropColumn("dbo.Position", "SchoolID");
            DropColumn("dbo.Position", "CityID");
            DropColumn("dbo.Posting", "SchoolID");
            DropColumn("dbo.Posting", "CityID");
            DropTable("dbo.School");
            DropTable("dbo.City");
        }
    }
}

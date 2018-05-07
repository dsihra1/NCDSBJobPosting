namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingSchoolCityCollection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posting", "CityID", "dbo.City");
            DropForeignKey("dbo.Posting", "SchoolID", "dbo.School");
            DropIndex("dbo.Posting", new[] { "CityID" });
            DropIndex("dbo.Posting", new[] { "SchoolID" });
            CreateTable(
                "dbo.CityPosting",
                c => new
                    {
                        City_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.City_ID, t.Posting_ID })
                .ForeignKey("dbo.City", t => t.City_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.City_ID)
                .Index(t => t.Posting_ID);
            
            CreateTable(
                "dbo.SchoolPosting",
                c => new
                    {
                        School_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.School_ID, t.Posting_ID })
                .ForeignKey("dbo.School", t => t.School_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.School_ID)
                .Index(t => t.Posting_ID);
            
            DropColumn("dbo.Posting", "CityID");
            DropColumn("dbo.Posting", "SchoolID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posting", "SchoolID", c => c.Int(nullable: false));
            AddColumn("dbo.Posting", "CityID", c => c.Int(nullable: false));
            DropForeignKey("dbo.SchoolPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.SchoolPosting", "School_ID", "dbo.School");
            DropForeignKey("dbo.CityPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.CityPosting", "City_ID", "dbo.City");
            DropIndex("dbo.SchoolPosting", new[] { "Posting_ID" });
            DropIndex("dbo.SchoolPosting", new[] { "School_ID" });
            DropIndex("dbo.CityPosting", new[] { "Posting_ID" });
            DropIndex("dbo.CityPosting", new[] { "City_ID" });
            DropTable("dbo.SchoolPosting");
            DropTable("dbo.CityPosting");
            CreateIndex("dbo.Posting", "SchoolID");
            CreateIndex("dbo.Posting", "CityID");
            AddForeignKey("dbo.Posting", "SchoolID", "dbo.School", "ID");
            AddForeignKey("dbo.Posting", "CityID", "dbo.City", "ID");
        }
    }
}

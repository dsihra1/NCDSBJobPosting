namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascade : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.School", new[] { "CityID" });
            AddColumn("dbo.City", "schoolID", c => c.Int(nullable: false));
            CreateIndex("dbo.School", "cityID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.School", new[] { "cityID" });
            DropColumn("dbo.City", "schoolID");
            CreateIndex("dbo.School", "CityID");
        }
    }
}

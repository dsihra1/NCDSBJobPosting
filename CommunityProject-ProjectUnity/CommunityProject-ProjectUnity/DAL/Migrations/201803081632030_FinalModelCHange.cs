namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalModelCHange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Position", new[] { "CityID" });
            DropIndex("dbo.Position", new[] { "SchoolID" });
            RenameColumn(table: "dbo.Position", name: "CityID", newName: "City_ID");
            RenameColumn(table: "dbo.Position", name: "SchoolID", newName: "School_ID");
            AddColumn("dbo.Position", "JobCode", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Position", "City_ID", c => c.Int());
            AlterColumn("dbo.Position", "School_ID", c => c.Int());
            CreateIndex("dbo.Position", "City_ID");
            CreateIndex("dbo.Position", "School_ID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Position", new[] { "School_ID" });
            DropIndex("dbo.Position", new[] { "City_ID" });
            AlterColumn("dbo.Position", "School_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Position", "City_ID", c => c.Int(nullable: false));
            DropColumn("dbo.Position", "JobCode");
            RenameColumn(table: "dbo.Position", name: "School_ID", newName: "SchoolID");
            RenameColumn(table: "dbo.Position", name: "City_ID", newName: "CityID");
            CreateIndex("dbo.Position", "SchoolID");
            CreateIndex("dbo.Position", "CityID");
        }
    }
}

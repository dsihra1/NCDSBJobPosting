namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserFileRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.aFile", "Application_ID", c => c.Int());
            CreateIndex("dbo.aFile", "Application_ID");
            AddForeignKey("dbo.aFile", "Application_ID", "dbo.Application", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.aFile", "Application_ID", "dbo.Application");
            DropIndex("dbo.aFile", new[] { "Application_ID" });
            DropColumn("dbo.aFile", "Application_ID");
        }
    }
}

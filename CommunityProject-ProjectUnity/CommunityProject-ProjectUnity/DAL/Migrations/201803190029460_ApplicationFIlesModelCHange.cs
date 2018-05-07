namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationFIlesModelCHange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.aFile", new[] { "Application_ID" });
            DropColumn("dbo.ApplicationFiles", "ApplicationID");
            RenameColumn(table: "dbo.ApplicationFiles", name: "Application_ID", newName: "ApplicationID");
            DropColumn("dbo.aFile", "Application_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.aFile", "Application_ID", c => c.Int());
            RenameColumn(table: "dbo.ApplicationFiles", name: "ApplicationID", newName: "Application_ID");
            AddColumn("dbo.ApplicationFiles", "ApplicationID", c => c.Int(nullable: false));
            CreateIndex("dbo.aFile", "Application_ID");
        }
    }
}

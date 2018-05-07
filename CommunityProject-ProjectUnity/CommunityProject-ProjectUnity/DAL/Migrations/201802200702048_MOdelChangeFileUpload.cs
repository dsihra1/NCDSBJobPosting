namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MOdelChangeFileUpload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Position", "JobType_ID", c => c.Int());
            CreateIndex("dbo.Position", "JobType_ID");
            AddForeignKey("dbo.Position", "JobType_ID", "dbo.JobType", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Position", "JobType_ID", "dbo.JobType");
            DropIndex("dbo.Position", new[] { "JobType_ID" });
            DropColumn("dbo.Position", "JobType_ID");
            DropTable("dbo.JobType");
        }
    }
}

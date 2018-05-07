namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTypeModelChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobTypePosition", "JobType_ID", "dbo.JobType");
            DropForeignKey("dbo.JobTypePosition", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.JobType", "Posting_ID", "dbo.Posting");
            DropIndex("dbo.JobType", new[] { "Posting_ID" });
            DropIndex("dbo.JobTypePosition", new[] { "JobType_ID" });
            DropIndex("dbo.JobTypePosition", new[] { "Position_ID" });
            AddColumn("dbo.Posting", "JobTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Posting", "JobTypeID");
            CreateIndex("dbo.Position", "JobTypeID");
            AddForeignKey("dbo.Position", "JobTypeID", "dbo.JobType", "ID");
            AddForeignKey("dbo.Posting", "JobTypeID", "dbo.JobType", "ID");
            DropColumn("dbo.JobType", "Posting_ID");
            DropTable("dbo.JobTypePosition");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobTypePosition",
                c => new
                    {
                        JobType_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobType_ID, t.Position_ID });
            
            AddColumn("dbo.JobType", "Posting_ID", c => c.Int());
            DropForeignKey("dbo.Posting", "JobTypeID", "dbo.JobType");
            DropForeignKey("dbo.Position", "JobTypeID", "dbo.JobType");
            DropIndex("dbo.Position", new[] { "JobTypeID" });
            DropIndex("dbo.Posting", new[] { "JobTypeID" });
            DropColumn("dbo.Posting", "JobTypeID");
            CreateIndex("dbo.JobTypePosition", "Position_ID");
            CreateIndex("dbo.JobTypePosition", "JobType_ID");
            CreateIndex("dbo.JobType", "Posting_ID");
            AddForeignKey("dbo.JobType", "Posting_ID", "dbo.Posting", "ID");
            AddForeignKey("dbo.JobTypePosition", "Position_ID", "dbo.Position", "ID", cascadeDelete: true);
            AddForeignKey("dbo.JobTypePosition", "JobType_ID", "dbo.JobType", "ID", cascadeDelete: true);
        }
    }
}

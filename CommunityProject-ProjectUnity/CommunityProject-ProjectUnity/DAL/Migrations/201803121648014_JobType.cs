namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Position", "JobTypeID", "dbo.JobType");
            DropIndex("dbo.Position", new[] { "JobTypeID" });
            CreateTable(
                "dbo.JobTypePosition",
                c => new
                    {
                        JobType_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobType_ID, t.Position_ID })
                .ForeignKey("dbo.JobType", t => t.JobType_ID, cascadeDelete: true)
                .ForeignKey("dbo.Position", t => t.Position_ID, cascadeDelete: true)
                .Index(t => t.JobType_ID)
                .Index(t => t.Position_ID);
            
            AddColumn("dbo.JobType", "JobCode", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.Position", "JobCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Position", "JobCode", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.JobTypePosition", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.JobTypePosition", "JobType_ID", "dbo.JobType");
            DropIndex("dbo.JobTypePosition", new[] { "Position_ID" });
            DropIndex("dbo.JobTypePosition", new[] { "JobType_ID" });
            DropColumn("dbo.JobType", "JobCode");
            DropTable("dbo.JobTypePosition");
            CreateIndex("dbo.Position", "JobTypeID");
            AddForeignKey("dbo.Position", "JobTypeID", "dbo.JobType", "ID");
        }
    }
}

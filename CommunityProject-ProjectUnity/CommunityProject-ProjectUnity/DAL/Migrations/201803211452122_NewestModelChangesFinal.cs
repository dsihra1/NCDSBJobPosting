namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewestModelChangesFinal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Position", "JobTypeID", "dbo.JobType");
            DropForeignKey("dbo.Posting", "JobTypeID", "dbo.JobType");
            DropIndex("dbo.Posting", new[] { "JobTypeID" });
            DropIndex("dbo.Position", new[] { "JobTypeID" });
            AddColumn("dbo.Qualification", "Position_ID", c => c.Int());
            AddColumn("dbo.Posting", "JobStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Posting", "JobEndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Position", "JobCode", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Skill", "Position_ID", c => c.Int());
            CreateIndex("dbo.Qualification", "Position_ID");
            CreateIndex("dbo.Skill", "Position_ID");
            AddForeignKey("dbo.Qualification", "Position_ID", "dbo.Position", "ID");
            AddForeignKey("dbo.Skill", "Position_ID", "dbo.Position", "ID");
            DropColumn("dbo.Posting", "JobTypeID");
            DropColumn("dbo.Position", "JobTypeID");
            DropTable("dbo.JobType");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JobType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        JobCode = c.String(nullable: false, maxLength: 10),
                        PositionType = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Position", "JobTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.Posting", "JobTypeID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Skill", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.Qualification", "Position_ID", "dbo.Position");
            DropIndex("dbo.Skill", new[] { "Position_ID" });
            DropIndex("dbo.Qualification", new[] { "Position_ID" });
            DropColumn("dbo.Skill", "Position_ID");
            DropColumn("dbo.Position", "JobCode");
            DropColumn("dbo.Posting", "JobEndDate");
            DropColumn("dbo.Posting", "JobStartDate");
            DropColumn("dbo.Qualification", "Position_ID");
            CreateIndex("dbo.Position", "JobTypeID");
            CreateIndex("dbo.Posting", "JobTypeID");
            AddForeignKey("dbo.Posting", "JobTypeID", "dbo.JobType", "ID");
            AddForeignKey("dbo.Position", "JobTypeID", "dbo.JobType", "ID");
        }
    }
}

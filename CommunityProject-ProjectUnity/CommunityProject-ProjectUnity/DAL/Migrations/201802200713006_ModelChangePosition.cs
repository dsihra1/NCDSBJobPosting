namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChangePosition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Position", "PostingID", "dbo.Posting");
            
            DropIndex("dbo.Posting", new[] { "Position_ID" });
            DropIndex("dbo.Position", new[] { "PostingID" });
            DropIndex("dbo.Position", new[] { "JobType_ID" });
           
           
            RenameColumn(table: "dbo.Position", name: "JobType_ID", newName: "JobTypeID");
            AlterColumn("dbo.Posting", "PositionID", c => c.Int(nullable: false));
            AlterColumn("dbo.Position", "JobTypeID", c => c.Int(nullable: false));
            
            CreateIndex("dbo.Position", "JobTypeID");
            DropColumn("dbo.Position", "PostingID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Position", "PostingID", c => c.Int(nullable: false));
            DropIndex("dbo.Position", new[] { "JobTypeID" });
           
            AlterColumn("dbo.Position", "JobTypeID", c => c.Int());
            AlterColumn("dbo.Posting", "PositionID", c => c.Int());
            RenameColumn(table: "dbo.Position", name: "JobTypeID", newName: "JobType_ID");
           
            AddColumn("dbo.Posting", "PositionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Position", "JobType_ID");
            CreateIndex("dbo.Position", "PostingID");
            CreateIndex("dbo.Posting", "Position_ID");
           
            AddForeignKey("dbo.Position", "PostingID", "dbo.Posting", "ID");
        }
    }
}

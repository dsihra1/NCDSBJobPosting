namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingControllerChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobType", "Posting_ID", c => c.Int());
            CreateIndex("dbo.JobType", "Posting_ID");
            AddForeignKey("dbo.JobType", "Posting_ID", "dbo.Posting", "ID");
            DropColumn("dbo.Posting", "JobCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posting", "JobCode", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.JobType", "Posting_ID", "dbo.Posting");
            DropIndex("dbo.JobType", new[] { "Posting_ID" });
            DropColumn("dbo.JobType", "Posting_ID");
        }
    }
}

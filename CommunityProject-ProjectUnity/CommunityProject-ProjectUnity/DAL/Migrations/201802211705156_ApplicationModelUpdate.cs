namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Application", "PostingID", "dbo.Posting");
            AddColumn("dbo.Application", "Posting_ID", c => c.Int());
            AddColumn("dbo.Posting", "Application_ID", c => c.Int());
            CreateIndex("dbo.Application", "Posting_ID");
            CreateIndex("dbo.Posting", "Application_ID");
            AddForeignKey("dbo.Posting", "Application_ID", "dbo.Application", "ID");
            AddForeignKey("dbo.Application", "Posting_ID", "dbo.Posting", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Application", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.Posting", "Application_ID", "dbo.Application");
            DropIndex("dbo.Posting", new[] { "Application_ID" });
            DropIndex("dbo.Application", new[] { "Posting_ID" });
            DropColumn("dbo.Posting", "Application_ID");
            DropColumn("dbo.Application", "Posting_ID");
            AddForeignKey("dbo.Application", "PostingID", "dbo.Posting", "ID");
        }
    }
}

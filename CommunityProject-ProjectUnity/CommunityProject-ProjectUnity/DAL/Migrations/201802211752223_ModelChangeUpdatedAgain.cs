namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChangeUpdatedAgain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Applicant", "Application_ID", "dbo.Application");
            
            DropIndex("dbo.Applicant", new[] { "Application_ID" });
           
           
           
           
           
            
            AlterColumn("dbo.Application", "PostingID", c => c.Int(nullable: false));
            AlterColumn("dbo.Application", "ApplicantID", c => c.Int(nullable: false));
            
            DropColumn("dbo.Applicant", "ApplicationID");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posting", "Application_ID", c => c.Int());
           
           
           
           
           
           
            CreateIndex("dbo.Posting", "Application_ID");
            CreateIndex("dbo.Application", "Applicant_ID");
            CreateIndex("dbo.Application", "Posting_ID");
           
           
        }
    }
}

namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingStatusChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posting", "status", c => c.String(nullable: false));
            DropColumn("dbo.Posting", "IsExpired");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posting", "IsExpired", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posting", "status");
        }
    }
}

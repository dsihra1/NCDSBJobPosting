namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posting", "OpeningCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posting", "OpeningCount");
        }
    }
}

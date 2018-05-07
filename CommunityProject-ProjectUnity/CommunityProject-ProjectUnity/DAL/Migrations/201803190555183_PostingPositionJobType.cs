namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingPositionJobType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posting", "PositionType", c => c.String());
            AddColumn("dbo.Position", "PositionType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Position", "PositionType");
            DropColumn("dbo.Posting", "PositionType");
        }
    }
}

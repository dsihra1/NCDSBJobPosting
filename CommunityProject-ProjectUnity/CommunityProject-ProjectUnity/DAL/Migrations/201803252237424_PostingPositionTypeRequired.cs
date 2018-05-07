namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostingPositionTypeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posting", "PositionType", c => c.String(nullable: false));
            AlterColumn("dbo.Position", "PositionType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Position", "PositionType", c => c.String());
            AlterColumn("dbo.Posting", "PositionType", c => c.String());
        }
    }
}

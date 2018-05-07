namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionTypeModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobType", "PositionType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobType", "PositionType");
        }
    }
}

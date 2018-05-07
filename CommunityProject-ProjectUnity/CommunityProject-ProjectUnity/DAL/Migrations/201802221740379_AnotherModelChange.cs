namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnotherModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Position", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Position", "PositionName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Position", "PositionName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Position", "Name");
        }
    }
}

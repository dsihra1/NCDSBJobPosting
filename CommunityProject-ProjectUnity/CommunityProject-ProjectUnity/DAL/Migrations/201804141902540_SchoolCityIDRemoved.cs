namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchoolCityIDRemoved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.School", "cityID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.School", "cityID", c => c.Int(nullable: false));
        }
    }
}

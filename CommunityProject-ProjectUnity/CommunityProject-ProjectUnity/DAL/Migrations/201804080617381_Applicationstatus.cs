namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Applicationstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Application", "Applicationstatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Application", "Applicationstatus");
        }
    }
}

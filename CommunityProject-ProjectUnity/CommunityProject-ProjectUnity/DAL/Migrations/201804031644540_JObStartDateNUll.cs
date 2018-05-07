namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JObStartDateNUll : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posting", "JobStartDate", c => c.DateTime());
            AlterColumn("dbo.Posting", "JobEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posting", "JobEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Posting", "JobStartDate", c => c.DateTime(nullable: false));
        }
    }
}

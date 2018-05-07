namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewerModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posting", "SalaryType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posting", "SalaryType");
        }
    }
}

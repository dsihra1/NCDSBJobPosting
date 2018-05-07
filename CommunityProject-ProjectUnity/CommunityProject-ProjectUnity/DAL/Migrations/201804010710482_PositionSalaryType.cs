namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionSalaryType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Position", "SalaryType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Position", "SalaryType");
        }
    }
}

namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalaryChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posting", "Salary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posting", "Salary", c => c.Int(nullable: false));
        }
    }
}

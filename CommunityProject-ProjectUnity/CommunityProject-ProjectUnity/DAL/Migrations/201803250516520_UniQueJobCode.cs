namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniQueJobCode : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Position", "JobCode", unique: true, name: "IX_Unique_JobCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Position", "IX_Unique_JobCode");
        }
    }
}

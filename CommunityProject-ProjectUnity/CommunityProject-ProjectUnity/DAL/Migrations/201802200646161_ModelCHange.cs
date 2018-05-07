namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelCHange : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Skill", name: "Posting_ID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Skill", name: "Posting_ID1", newName: "Posting_ID");
            RenameColumn(table: "dbo.Skill", name: "__mig_tmp__0", newName: "Posting_ID1");
            RenameIndex(table: "dbo.Skill", name: "IX_Posting_ID1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Skill", name: "IX_Posting_ID", newName: "IX_Posting_ID1");
            RenameIndex(table: "dbo.Skill", name: "__mig_tmp__0", newName: "IX_Posting_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Skill", name: "IX_Posting_ID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Skill", name: "IX_Posting_ID1", newName: "IX_Posting_ID");
            RenameIndex(table: "dbo.Skill", name: "__mig_tmp__0", newName: "IX_Posting_ID1");
            RenameColumn(table: "dbo.Skill", name: "Posting_ID1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Skill", name: "Posting_ID", newName: "Posting_ID1");
            RenameColumn(table: "dbo.Skill", name: "__mig_tmp__0", newName: "Posting_ID");
        }
    }
}

namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChangeUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posting", "Skill_ID", "dbo.Skill");
            DropForeignKey("dbo.Skill", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.Skill", "Posting_ID1", "dbo.Posting");
            DropIndex("dbo.Posting", new[] { "Skill_ID" });
            DropIndex("dbo.Skill", new[] { "Posting_ID" });
            DropIndex("dbo.Skill", new[] { "Posting_ID1" });
            CreateTable(
                "dbo.SkillPosting",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Posting_ID })
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Posting_ID);
            
            DropColumn("dbo.Posting", "Skill_ID");
            DropColumn("dbo.Skill", "Posting_ID");
            DropColumn("dbo.Skill", "Posting_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skill", "Posting_ID1", c => c.Int());
            AddColumn("dbo.Skill", "Posting_ID", c => c.Int());
            AddColumn("dbo.Posting", "Skill_ID", c => c.Int());
            DropForeignKey("dbo.SkillPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.SkillPosting", "Skill_ID", "dbo.Skill");
            DropIndex("dbo.SkillPosting", new[] { "Posting_ID" });
            DropIndex("dbo.SkillPosting", new[] { "Skill_ID" });
            DropTable("dbo.SkillPosting");
            CreateIndex("dbo.Skill", "Posting_ID1");
            CreateIndex("dbo.Skill", "Posting_ID");
            CreateIndex("dbo.Posting", "Skill_ID");
            AddForeignKey("dbo.Skill", "Posting_ID1", "dbo.Posting", "ID");
            AddForeignKey("dbo.Skill", "Posting_ID", "dbo.Posting", "ID");
            AddForeignKey("dbo.Posting", "Skill_ID", "dbo.Skill", "ID");
        }
    }
}

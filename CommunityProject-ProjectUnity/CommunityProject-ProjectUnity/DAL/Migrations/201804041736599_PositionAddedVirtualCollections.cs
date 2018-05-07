namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PositionAddedVirtualCollections : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Qualification", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.Skill", "Position_ID", "dbo.Position");
            DropIndex("dbo.Qualification", new[] { "Position_ID" });
            DropIndex("dbo.Skill", new[] { "Position_ID" });
            CreateTable(
                "dbo.SkillPosition",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Position_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Position_ID })
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.Position", t => t.Position_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Position_ID);
            
            CreateTable(
                "dbo.PositionQualification",
                c => new
                    {
                        Position_ID = c.Int(nullable: false),
                        Qualification_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Position_ID, t.Qualification_ID })
                .ForeignKey("dbo.Position", t => t.Position_ID, cascadeDelete: true)
                .ForeignKey("dbo.Qualification", t => t.Qualification_ID, cascadeDelete: true)
                .Index(t => t.Position_ID)
                .Index(t => t.Qualification_ID);
            
            DropColumn("dbo.Qualification", "Position_ID");
            DropColumn("dbo.Skill", "Position_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skill", "Position_ID", c => c.Int());
            AddColumn("dbo.Qualification", "Position_ID", c => c.Int());
            DropForeignKey("dbo.PositionQualification", "Qualification_ID", "dbo.Qualification");
            DropForeignKey("dbo.PositionQualification", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.SkillPosition", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.SkillPosition", "Skill_ID", "dbo.Skill");
            DropIndex("dbo.PositionQualification", new[] { "Qualification_ID" });
            DropIndex("dbo.PositionQualification", new[] { "Position_ID" });
            DropIndex("dbo.SkillPosition", new[] { "Position_ID" });
            DropIndex("dbo.SkillPosition", new[] { "Skill_ID" });
            DropTable("dbo.PositionQualification");
            DropTable("dbo.SkillPosition");
            CreateIndex("dbo.Skill", "Position_ID");
            CreateIndex("dbo.Qualification", "Position_ID");
            AddForeignKey("dbo.Skill", "Position_ID", "dbo.Position", "ID");
            AddForeignKey("dbo.Qualification", "Position_ID", "dbo.Position", "ID");
        }
    }
}

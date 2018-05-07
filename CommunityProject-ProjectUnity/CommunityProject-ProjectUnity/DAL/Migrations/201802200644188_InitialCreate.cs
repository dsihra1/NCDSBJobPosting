namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 150),
                        Phone = c.String(nullable: false),
                        QualificationID = c.Int(nullable: false),
                        ApplicationID = c.Int(nullable: false),
                        PostingID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Application_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Application", t => t.Application_ID)
                .Index(t => t.Email, unique: true, name: "IX_Uniqe_Email")
                .Index(t => t.Application_ID);
            
            CreateTable(
                "dbo.ApplicantImage",
                c => new
                    {
                        ApplicantImageID = c.Int(nullable: false),
                        imageContent = c.Binary(),
                        imageMimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ApplicantImageID)
                .ForeignKey("dbo.Applicant", t => t.ApplicantImageID, cascadeDelete: true)
                .Index(t => t.ApplicantImageID);
            
            CreateTable(
                "dbo.Application",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Comments = c.String(nullable: false, maxLength: 2000),
                        PostingID = c.Int(nullable: false),
                        ApplicantID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Applicant_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Applicant", t => t.ApplicantID)
                .ForeignKey("dbo.Posting", t => t.PostingID)
                .ForeignKey("dbo.Applicant", t => t.Applicant_ID)
                .Index(t => new { t.PostingID, t.ApplicantID }, unique: true, name: "IX_Unique_Application")
                .Index(t => t.Applicant_ID);
            
            CreateTable(
                "dbo.aFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fileName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                        ApplicantID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Application_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Application", t => t.Application_ID)
                .ForeignKey("dbo.Applicant", t => t.ApplicantID, cascadeDelete: true)
                .Index(t => t.ApplicantID)
                .Index(t => t.Application_ID);
            
            CreateTable(
                "dbo.FileContent",
                c => new
                    {
                        FileContentID = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.FileContentID)
                .ForeignKey("dbo.aFile", t => t.FileContentID, cascadeDelete: true)
                .Index(t => t.FileContentID);
            
            CreateTable(
                "dbo.Posting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        JobCode = c.String(nullable: false, maxLength: 10),
                        StartDate = c.DateTime(),
                        ClosingDate = c.DateTime(nullable: false),
                        IsExpired = c.Boolean(nullable: false),
                        City = c.String(nullable: false, maxLength: 50),
                        FTE = c.Double(nullable: false),
                        Openings = c.Int(nullable: false),
                        Salary = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 1200),
                        SchoolName = c.String(nullable: false, maxLength: 50),
                        PositionID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Position_ID = c.Int(),
                        Skill_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Position", t => t.Position_ID)
                .ForeignKey("dbo.Position", t => t.PositionID)
                .ForeignKey("dbo.Skill", t => t.Skill_ID)
                .Index(t => t.PositionID)
                .Index(t => t.Position_ID)
                .Index(t => t.Skill_ID);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PositionName = c.String(nullable: false, maxLength: 50),
                        PositionDescription = c.String(nullable: false, maxLength: 1200),
                        PositionSalary = c.Int(nullable: false),
                        PositionCity = c.String(nullable: false, maxLength: 50),
                        PositionSchoolName = c.String(nullable: false, maxLength: 50),
                        PostingID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posting", t => t.PostingID)
                .Index(t => t.PostingID);
            
            CreateTable(
                "dbo.Qualification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 500),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true, name: "IX_Unique_Qualification");
            
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SkillName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 500),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Posting_ID = c.Int(),
                        Posting_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posting", t => t.Posting_ID)
                .ForeignKey("dbo.Posting", t => t.Posting_ID1)
                .Index(t => t.SkillName, unique: true, name: "IX_Unique_Skill")
                .Index(t => t.Posting_ID)
                .Index(t => t.Posting_ID1);
            
            CreateTable(
                "dbo.QualificationApplicant",
                c => new
                    {
                        Qualification_ID = c.Int(nullable: false),
                        Applicant_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Qualification_ID, t.Applicant_ID })
                .ForeignKey("dbo.Qualification", t => t.Qualification_ID, cascadeDelete: true)
                .ForeignKey("dbo.Applicant", t => t.Applicant_ID, cascadeDelete: true)
                .Index(t => t.Qualification_ID)
                .Index(t => t.Applicant_ID);
            
            CreateTable(
                "dbo.QualificationPosting",
                c => new
                    {
                        Qualification_ID = c.Int(nullable: false),
                        Posting_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Qualification_ID, t.Posting_ID })
                .ForeignKey("dbo.Qualification", t => t.Qualification_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posting", t => t.Posting_ID, cascadeDelete: true)
                .Index(t => t.Qualification_ID)
                .Index(t => t.Posting_ID);
            
            CreateTable(
                "dbo.SkillApplicant",
                c => new
                    {
                        Skill_ID = c.Int(nullable: false),
                        Applicant_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Skill_ID, t.Applicant_ID })
                .ForeignKey("dbo.Skill", t => t.Skill_ID, cascadeDelete: true)
                .ForeignKey("dbo.Applicant", t => t.Applicant_ID, cascadeDelete: true)
                .Index(t => t.Skill_ID)
                .Index(t => t.Applicant_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.aFile", "ApplicantID", "dbo.Applicant");
            DropForeignKey("dbo.Application", "Applicant_ID", "dbo.Applicant");
            DropForeignKey("dbo.Skill", "Posting_ID1", "dbo.Posting");
            DropForeignKey("dbo.Skill", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.Posting", "Skill_ID", "dbo.Skill");
            DropForeignKey("dbo.SkillApplicant", "Applicant_ID", "dbo.Applicant");
            DropForeignKey("dbo.SkillApplicant", "Skill_ID", "dbo.Skill");
            DropForeignKey("dbo.QualificationPosting", "Posting_ID", "dbo.Posting");
            DropForeignKey("dbo.QualificationPosting", "Qualification_ID", "dbo.Qualification");
            DropForeignKey("dbo.QualificationApplicant", "Applicant_ID", "dbo.Applicant");
            DropForeignKey("dbo.QualificationApplicant", "Qualification_ID", "dbo.Qualification");
            DropForeignKey("dbo.Posting", "PositionID", "dbo.Position");
            DropForeignKey("dbo.Posting", "Position_ID", "dbo.Position");
            DropForeignKey("dbo.Position", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.Application", "PostingID", "dbo.Posting");
            DropForeignKey("dbo.aFile", "Application_ID", "dbo.Application");
            DropForeignKey("dbo.FileContent", "FileContentID", "dbo.aFile");
            DropForeignKey("dbo.Applicant", "Application_ID", "dbo.Application");
            DropForeignKey("dbo.Application", "ApplicantID", "dbo.Applicant");
            DropForeignKey("dbo.ApplicantImage", "ApplicantImageID", "dbo.Applicant");
            DropIndex("dbo.SkillApplicant", new[] { "Applicant_ID" });
            DropIndex("dbo.SkillApplicant", new[] { "Skill_ID" });
            DropIndex("dbo.QualificationPosting", new[] { "Posting_ID" });
            DropIndex("dbo.QualificationPosting", new[] { "Qualification_ID" });
            DropIndex("dbo.QualificationApplicant", new[] { "Applicant_ID" });
            DropIndex("dbo.QualificationApplicant", new[] { "Qualification_ID" });
            DropIndex("dbo.Skill", new[] { "Posting_ID1" });
            DropIndex("dbo.Skill", new[] { "Posting_ID" });
            DropIndex("dbo.Skill", "IX_Unique_Skill");
            DropIndex("dbo.Qualification", "IX_Unique_Qualification");
            DropIndex("dbo.Position", new[] { "PostingID" });
            DropIndex("dbo.Posting", new[] { "Skill_ID" });
            DropIndex("dbo.Posting", new[] { "Position_ID" });
            DropIndex("dbo.Posting", new[] { "PositionID" });
            DropIndex("dbo.FileContent", new[] { "FileContentID" });
            DropIndex("dbo.aFile", new[] { "Application_ID" });
            DropIndex("dbo.aFile", new[] { "ApplicantID" });
            DropIndex("dbo.Application", new[] { "Applicant_ID" });
            DropIndex("dbo.Application", "IX_Unique_Application");
            DropIndex("dbo.ApplicantImage", new[] { "ApplicantImageID" });
            DropIndex("dbo.Applicant", new[] { "Application_ID" });
            DropIndex("dbo.Applicant", "IX_Uniqe_Email");
            DropTable("dbo.SkillApplicant");
            DropTable("dbo.QualificationPosting");
            DropTable("dbo.QualificationApplicant");
            DropTable("dbo.Skill");
            DropTable("dbo.Qualification");
            DropTable("dbo.Position");
            DropTable("dbo.Posting");
            DropTable("dbo.FileContent");
            DropTable("dbo.aFile");
            DropTable("dbo.Application");
            DropTable("dbo.ApplicantImage");
            DropTable("dbo.Applicant");
        }
    }
}

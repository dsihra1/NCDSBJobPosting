namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationFilesFinalRelation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.QualificationPosting", newName: "PostingQualification");
            DropPrimaryKey("dbo.PostingQualification");
            CreateTable(
                "dbo.ApplicationFilesContent",
                c => new
                    {
                        FileContentID = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.FileContentID)
                .ForeignKey("dbo.ApplicationFiles", t => t.FileContentID)
                .Index(t => t.FileContentID);
            
            CreateTable(
                "dbo.ApplicationFiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fileName = c.String(maxLength: 256),
                        Description = c.String(maxLength: 256),
                        ApplicationID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Application", t => t.ApplicationID)
                .Index(t => t.ApplicationID);
            
            AddPrimaryKey("dbo.PostingQualification", new[] { "Posting_ID", "Qualification_ID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationFilesContent", "FileContentID", "dbo.ApplicationFiles");
            DropForeignKey("dbo.ApplicationFiles", "ApplicationID", "dbo.Application");
            DropIndex("dbo.ApplicationFiles", new[] { "ApplicationID" });
            DropIndex("dbo.ApplicationFilesContent", new[] { "FileContentID" });
            DropPrimaryKey("dbo.PostingQualification");
            DropTable("dbo.ApplicationFiles");
            DropTable("dbo.ApplicationFilesContent");
            AddPrimaryKey("dbo.PostingQualification", new[] { "Qualification_ID", "Posting_ID" });
            RenameTable(name: "dbo.PostingQualification", newName: "QualificationPosting");
        }
    }
}

using CommunityProject_ProjectUnity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.DAL
{
    public class ProjectUnityEntities : DbContext
    {
        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<aFile> Files { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Posting> Postings { get; set; }

        public DbSet<Qualification> Qualifications { get; set; }

        public DbSet<Skill> Skills { get; set; }

       

        public DbSet<School> Schools { get; set; }

        public DbSet<ApplicationFiles> AppFiles { get; set; }

        public DbSet<ApplicationFilesContent> AppFileContent { get; set; }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Added for cascade delete for applicant image profile picture
            modelBuilder.Entity<Applicant>()
                .HasOptional(w => w.ApplicantImage)
                .WithRequired(p => p.Applicant)
                .WillCascadeOnDelete(true);

            //Added for cascade delte for all Files with Applicant
            modelBuilder.Entity<Applicant>()
                .HasMany(a => a.Files)
                .WithRequired(p => p.Applicant)
                .WillCascadeOnDelete(true);

            //Added for cascade delete for File Content with File
            modelBuilder.Entity<aFile>()
                .HasOptional(w => w.FileContent)
                .WithRequired(p => p.aFile)
                .WillCascadeOnDelete(true);

        }

        public override int SaveChanges()
        {
            //Get Audit Values if not supplied
            string auditUser = "Anonymous";
            try //Need to try becuase HttpContext might not exist
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    auditUser = HttpContext.Current.User.Identity.Name;
            }
            catch (Exception)
            { }

            DateTime auditDate = DateTime.UtcNow;
            foreach (DbEntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = auditDate;
                    entry.Entity.CreatedBy = auditUser;
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
            }
            return base.SaveChanges();
        }

        

        public System.Data.Entity.DbSet<CommunityProject_ProjectUnity.Models.ApplicantImage> ApplicantImages { get; set; }

        public System.Data.Entity.DbSet<CommunityProject_ProjectUnity.Models.FileContent> FileContents { get; set; }
    }
}
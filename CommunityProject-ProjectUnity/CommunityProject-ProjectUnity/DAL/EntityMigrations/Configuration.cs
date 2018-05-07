namespace CommunityProject_ProjectUnity.DAL.EntityMigrations
{
    using CommunityProject_ProjectUnity.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<CommunityProject_ProjectUnity.DAL.ProjectUnityEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\Migrations";
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                );
            }
            catch (Exception e)
            {
                throw new Exception(
                     "Seed Failed - errors follow:\n" +
                     e.InnerException.InnerException.Message.ToString(), e
                 );
            }
        }

        protected override void Seed(CommunityProject_ProjectUnity.DAL.ProjectUnityEntities context)
        {

            //var postings = new List<Posting>
            //{
            //    new Posting { Name="School Nurse", ClosingDate = DateTime.Parse("2018-06-03"), StartDate = DateTime.Parse("2018-08-03"), City = "Grimsby", Openings=1,IsExpired=false,FTE=0.40,
            //        Description = "Wanted School Nurse! Must have at least a college diploma, 1+ years experience as Nurse, and Driver Licence", Salary=49000, SchoolName="Our Lady of Fatima", JobCode = "NURS003"},
            //    new Posting { Name="French Teacher", ClosingDate = DateTime.Parse("2018-06-03"), StartDate = DateTime.Parse("2018-08-03"), City = "Pelham", Openings=1,IsExpired=false,FTE=0.60,
            //        Description = "Wanted French Teacher! Must be able to speak both French and English fluently, graduate from teachers college, and French teaching and/or tutoring experience",
            //        Salary =65000, SchoolName = "St. Ann", JobCode = "FREN004"},
            //    new Posting { Name="English Teacher", ClosingDate = DateTime.Parse("2018-06-03"), StartDate = DateTime.Parse("2018-08-03"), City = "Welland", Openings=1,IsExpired=false,FTE=0.30,
            //        Description = "We are looking for a talented teaching professional passionate about helping students reach their full potential.", Salary=89000, SchoolName = "Alexander Kuska", JobCode = "ENGL005"},
            //    new Posting { Name="Chemistry Teacher", ClosingDate = DateTime.Parse("2018-06-03"), StartDate = DateTime.Parse("2018-08-03"), City = "West Lincoln", Openings=1,IsExpired=false,FTE=0.50,
            //        Description = "We are looking for a Chemist expert, who is willing to teach students from grade 11-12 about Chemistry.", Salary=89000, SchoolName = "St. John", JobCode = "CHEM001"}


            //};
            //postings.ForEach(a => context.Postings.AddOrUpdate(n => n.Name, a));
            //SaveChanges(context);

            //var positions = new List<Position>
            //{


            //     new Position {PositionName="School Nurse", PositionDescription="Wanted School Nurse! Must have at least a college diploma, 1+ years experience as Nurse, and Driver Licence",
            //     PositionSalary=49000,  PositionCity="Grimsby",PositionSchoolName="Our Lady of Fatima",  PostingID=(context.Postings.Where(p=>p.Name=="Full-Time School Nurse").SingleOrDefault().ID)},
            //    new Position { PositionName="French Teacher", PositionDescription="Wanted French Teacher! Must be able to speak both French and English fluently, graduate from teachers college," +
            //    " and French teaching and/or tutoring experience",
            //     PositionSalary=65000,  PositionCity="Pelham",PositionSchoolName="St. Ann",  PostingID=(context.Postings.Where(p=>p.Name=="Part-Time French Teacher").SingleOrDefault().ID)},
            //    new Position { PositionName="English Teacher", PositionDescription="We are looking for a talented teaching professional passionate about helping students reach their full potential.",
            //     PositionSalary=89000,  PositionCity="Welland",PositionSchoolName="Alexander Kuska",  PostingID=(context.Postings.Where(p=>p.Name=="Part-Time English Teacher").SingleOrDefault().ID)},
            //     new Position {  PositionName="Chemistry Teacher", PositionDescription="We are looking for a Chemist expert, who is willing to teach students from grade 11-12 about Chemistry.",
            //     PositionSalary=89000,  PositionCity="West Lincoln",PositionSchoolName="St. John",  PostingID=(context.Postings.Where(p=>p.Name=="Full-Time Chemistry Teacher").SingleOrDefault().ID)}

            //};
            //positions.ForEach(a => context.Positions.AddOrUpdate(n => n.PositionName, a));
            //SaveChanges(context);

            //var applicants = new List<Applicant>
            //{
            //    new Applicant { FirstName = "Fred", LastName = "Stoneburg",
            //        Email="fred.stoneburg@outlook.com", Phone="9052322222"},
            //    new Applicant { FirstName = "Wilma", LastName = "Jones",
            //        Phone ="9055551212", Email="wjones@outlook.com" },
            //    new Applicant { FirstName = "Barney", LastName = "Banks", Phone="9055551213", Email="banks.Barney@outlook.com" },
            //    new Applicant { FirstName = "Joe", LastName = "Smith",
            //        Phone ="9055554321", Email="jjs@outlook.com" }

            //};
            //applicants.ForEach(a => context.Applicants.AddOrUpdate(n => n.Email, a));
            //SaveChanges(context);


            //var applications = new List<Application>
            //{
            //    new Application { ApplicantID=(context.Applicants.Where(p=>p.Email=="fred.stoneburg@outlook.com").SingleOrDefault().ID),
            //        PostingID=(context.Postings.Where(p=>p.Name=="Part-Time French Teacher").SingleOrDefault().ID) },
            //    new Application { ApplicantID=(context.Applicants.Where(p=>p.Email=="banks.barney@outlook.com").SingleOrDefault().ID),
            //        PostingID=(context.Postings.Where(p=>p.Name=="Full-Time Chemistry Teacher").SingleOrDefault().ID)},
            //    new Application { ApplicantID=(context.Applicants.Where(p=>p.Email=="wjones@outlook.com").SingleOrDefault().ID),
            //         PostingID=(context.Postings.Where(p=>p.Name=="Full-Time School Nurse").SingleOrDefault().ID) }
            //};
            //applications.ForEach(a => context.Applications.AddOrUpdate(n => n.Posting));
            //SaveChanges(context);

            ////var qualifications = new List<Qualification>
            ////    {
            ////        new Qualification {Name="French language", Description="Ontario Certified teacher with minimal 2-3 years experienced and be able to teach primary, junior and/or senior French as a second language (FSL) or French native speaker, and Strong English written and oral communication skills",
            ////        ApplicantID=(context.Applicants.Where(p=>p.Email=="fred.stoneburg@outlook.com").SingleOrDefault().ID),
            ////        PostingID=(context.Postings.Where(p=>p.Name=="Part-Time French Teacher").SingleOrDefault().ID)},

            ////        new Qualification {Name="Chemoistry",Description="Able to apply Differentiated Instructions to accommodate students' different learning needs and backgrounds. Passionate about teaching, consulting and mentoring. professional and dependable",
            ////        ApplicantID=(context.Applicants.Where(p=>p.Email=="banks.barney@outlook.com").SingleOrDefault().ID),
            ////        PostingID=(context.Postings.Where(p=>p.Name=="Full-Time Chemistry Teacher").SingleOrDefault().ID)},

            ////        new Qualification {Name="Nursing", Description="Must have a minimum of two years of nursing experience working with the general public (hospital, home care, other cosmetic clinic, etc.)",
            ////        ApplicantID=(context.Applicants.Where(p=>p.Email=="wjones@outlook.com").SingleOrDefault().ID),
            ////        PostingID=(context.Postings.Where(p=>p.Name=="Full-Time School Nurse").SingleOrDefault().ID)}
            ////    };
            ////qualifications.ForEach(a => context.Qualifications.AddOrUpdate(n => n.Name, a));

            ////var skill = new List<Skill>
            ////{

            ////};
            ////SaveChanges(context);
        }
    }
}

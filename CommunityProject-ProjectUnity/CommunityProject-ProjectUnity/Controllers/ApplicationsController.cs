using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityProject_ProjectUnity.DAL;
using CommunityProject_ProjectUnity.Models;
using CommunityProject_ProjectUnity.ViewModels;
using Microsoft.AspNet.Identity;
using Rotativa;

namespace CommunityProject_ProjectUnity.Controllers
{
   
    public class ApplicationsController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Applications
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Applicant).Include(a => a.Posting);
            if (!User.IsInRole("Admin"))
            {
                  applications = db.Applications.Include(a => a.Applicant).Include(a => a.Posting)
                                              .Where(a => a.CreatedBy == User.Identity.Name);
            }
            
                
            return View(applications.ToList());
        }
        public ActionResult SelectedApplications()
        {
            var applications = db.Applications.Where(a => a.Applicationstatus == "Selected");
            return View(applications.ToList());
        }
        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            var userfiles = db.AppFiles.Where(a => a.ApplicationID == id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            var postingID = application.PostingID;
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.Files = userfiles.ToList();

            var skills = application.Applicant.Skills;
            //ViewBag.SkillsList = skills.ToList();
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create(int?id)
        {
            var application = new Application();
            int userID = db.Applicants
                       .Where(q => q.Email==User.Identity.Name.ToString())
                       .Select(q => q.ID)
                       .FirstOrDefault();
            Posting posting = db.Postings.Find(id);
            string posname = posting.Name;
            var userfiles = db.Files.Where(a => a.ApplicantID == userID);
            ViewBag.ApplicantID = userID;
            ViewBag.PostingName = posname;
            //ViewBag.City = posting.City.Name;
            ViewBag.PostingID = id;
            //ViewBag.School = posting.School.Name;
            ViewBag.Desc = posting.Description;
            PopulateUserFileData(application);
            var poschools = posting.Schools.ToList();
            ViewBag.Schools = poschools;
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Comments,PostingID,ApplicantID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Application application, string [] selectedFiles,
            IEnumerable<HttpPostedFileBase> Attachments)
        {
            if (ModelState.IsValid)
            {
                //Add the selected skills
                if (selectedFiles != null)
                {
                    application.UserFiles = new List<aFile>();
                    foreach (var file in selectedFiles)
                    {
                        var FileToAdd = db.Files.Find(int.Parse(file));
                        application.UserFiles.Add(FileToAdd);
                    }
                }
                AddDocuments(ref application, Attachments);
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Details", "Applicants");
            }

            //ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
            //ViewBag.PostingID = new SelectList(db.Postings, "ID", "Name", application.PostingID);
            return View(application);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            int userID = db.Applicants
                      .Where(q => q.Email == User.Identity.Name.ToString())
                      .Select(q => q.ID)
                      .FirstOrDefault();
            int postid = db.Applications
                           .Where(p => p.ID == id)
                           .Select(q => q.PostingID)
                           .FirstOrDefault();
            var userfiles = db.Files.Where(a => a.ApplicantID == userID);
            Posting posting = db.Postings.Find(postid);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
            //ViewBag.PostingID = new SelectList(db.Postings, "ID", "Name", application.PostingID);
            ViewBag.ApplicantID = userID;
            ViewBag.PostingID = postid;
            ViewBag.PostingName = posting.Name;
            ViewBag.Desc = posting.Description;
            var poschools = posting.Schools.ToList();
            ViewBag.Schools = poschools;
            ViewBag.Files = userfiles.ToList();
            ViewBag.ApplicationStatus = new SelectList(ApplicationStatus, "Value", "Text", application?.Applicationstatus);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
       
        public ActionResult EditPost(int? id, Byte[] rowVersion, IEnumerable<HttpPostedFileBase> Attachments)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var applicationToUpdate = db.Applications
                                        .Include(a => a.Files)
                                        .Where(p => p.ID == id).SingleOrDefault();
            //AddDocuments(ref applicationToUpdate, Attachments);
            if (TryUpdateModel(applicationToUpdate, "",
              new string[] { "Comments", "PostingID", "ApplicantID", "Applicationstatus" }))
            {
                try
                {
                    db.Entry(applicationToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Application)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Posting was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Application)databaseEntry.ToObject();
                        
                        
                        if (databaseValues.Comments != clientValues.Comments)
                            ModelState.AddModelError("Comments", "Current value: "
                                + databaseValues.Comments);
                        
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        applicationToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            ViewBag.ApplicationStatus = new SelectList(ApplicationStatus, "Value", "Text", applicationToUpdate?.Applicationstatus);
            return View(applicationToUpdate);
        }
        //ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", application.ApplicantID);
        //ViewBag.PostingID = new SelectList(db.Postings, "ID", "Name", application.PostingID);


        List<SelectListItem> ApplicationStatus = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Short-Listed", Value = "Short-Listed" },
                new SelectListItem { Text = "In Process", Value = "In Process" },
                new SelectListItem { Text = "Interview Call Sent", Value = "Interview Call Sent" },
                new SelectListItem { Text = "Selected for Job", Value = "Selected" },


            };

        public ActionResult PrintPDF(int?id)
        {
            return new ActionAsPdf("Details", new { id }) { FileName = "Application.pdf" };
        }
        // GET: Applications/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Application application = db.Applications.Find(id);
        //    if (application == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(application);
        //}

        //// POST: Applications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Application application = db.Applications.Find(id);
        //    db.Applications.Remove(application);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        private void PopulateUserFileData(Application application)
        {
            int userID = db.Applicants
                       .Where(q => q.Email == User.Identity.Name.ToString())
                       .Select(q => q.ID)
                       .FirstOrDefault();
            var allfiles = db.Files.Where(a => a.ApplicantID == userID);
            var appFiles = new HashSet<int>(application.UserFiles.Select(a =>a.ID));
            var viewModel = new List<AssignedUserFilesVM>();
            foreach (var af in allfiles)
            {
                viewModel.Add(new AssignedUserFilesVM
                {
                    FileID = af.ID,
                    FileName = af.fileName,
                    Assigned = appFiles.Contains(af.ID)
                });
            }
            ViewBag.Files = viewModel;
        }
        public FileContentResult Download(int id)
        {
            var theFile = db.Files.Include(f => f.FileContent).Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent.Content, theFile.FileContent.MimeType, theFile.fileName);
        }

        private void AddDocuments(ref Application application, IEnumerable<HttpPostedFileBase> docs)
        {
            foreach (var f in docs)
            {
                if (f != null)
                {
                    string mimeType = f.ContentType;
                    string fileName = Path.GetFileName(f.FileName);
                    int fileLength = f.ContentLength;
                    //Note: you could filter for mime types if you only want to allow
                    //certain types of files.  I am allowing everything.
                    if (!(fileName == "" || fileLength == 0))//Looks like we have a file!!!
                    {
                        Stream fileStream = f.InputStream;
                        byte[] fileData = new byte[fileLength];
                        fileStream.Read(fileData, 0, fileLength);

                        ApplicationFiles newFile = new ApplicationFiles
                        {
                            FilesContent = new ApplicationFilesContent
                            {
                                Content = fileData,
                                MimeType = mimeType
                            },
                            fileName = fileName
                        };
                        application.Files.Add(newFile);
                    };
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

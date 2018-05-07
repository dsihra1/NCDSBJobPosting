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
using PagedList;

namespace CommunityProject_ProjectUnity.Controllers
{
    [Authorize]
    public class ApplicantsController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Applicants
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }

        // GET: Applicants/Details/5
        public ActionResult Details(int? id)
        {
            Applicant applicant = db.Applicants.Include(p =>p.Files)
                .Include(p =>p.Applications)
                .Include(p =>p.ApplicantImage)
                .Where(c => c.Email == User.Identity.Name).SingleOrDefault();
            //var applicant = db.Applicants.AsNoTracking()
            //    .Include(p => p.Files)
            //    .Include(p => p.Skills)
            //    .Where(p => p.Email == User.Identity.Name);

            if (applicant == null)
            {
                return RedirectToAction("Create");
            }
          
            return View(applicant);
        }

        // GET: Applicants/Create
        public ActionResult Create()
        {
            //Add all (unchecked) Skills to ViewBag
            var applicant = new Applicant();
            applicant.Skills = new List<Skill>();
            PopulateAssignedSkillData(applicant);
            return View();
        }

        // POST: Applicants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,MiddleName,LastName,Phone,Email")] Applicant applicant, 
            string[] selectedSkills, IEnumerable<HttpPostedFileBase> Attachments)
        {
            try
            {
                //Add the selected skills
                if (selectedSkills != null)
                {
                    applicant.Skills = new List<Skill>();
                    foreach (var skill in selectedSkills)
                    {
                        var skillToAdd = db.Skills.Find(int.Parse(skill));
                        applicant.Skills.Add(skillToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    AddPicture(ref applicant, Request.Files["ProfilePicture"]);
                    AddDocuments(ref applicant, Attachments);
                    db.Applicants.Add(applicant);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                {
                    ModelState.AddModelError("eMail", "Unable to save changes. Remember, no two applicants can have the same eMail address.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again.");
                }
            }

            return View(applicant);
        }

        // GET: Applicants/Edit/5
        public ActionResult Edit(int? id)
        {
            Applicant applicant = db.Applicants.Include(p => p.Files)
                .Include(p => p.Applications)
                .Include(p => p.ApplicantImage)
                .Where(c => c.Email == User.Identity.Name).SingleOrDefault();
            if (applicant == null)
            {
                return RedirectToAction("Create");
            }
           
            PopulateAssignedSkillData(applicant);
            return View(applicant);
        }

        // POST: Applicants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion, 
            string[] selectedSkills, string chkRemoveImage, IEnumerable<HttpPostedFileBase> Attachments)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicantToUpdate = db.Applicants
                .Include(a => a.Skills).Include(a => a.Files)
                .Include(a => a.ApplicantImage)
               .Where(c => c.Email == User.Identity.Name).SingleOrDefault();
            if (TryUpdateModel(applicantToUpdate, "",
                new string[] { "FirstName", "MiddleName", "LastName", "Phone",}))
            {
                try
                {
                    UpdateApplicantSkills(selectedSkills, applicantToUpdate);
                    if (chkRemoveImage != null)
                    {
                        applicantToUpdate.ApplicantImage = null;
                    }
                    else
                    {
                        AddPicture(ref applicantToUpdate, Request.Files["ProfilePicture"]);
                    }
                    AddDocuments(ref applicantToUpdate, Attachments);
                    db.Entry(applicantToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Applicant)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Applicant was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Applicant)databaseEntry.ToObject();
                        if (databaseValues.FirstName != clientValues.FirstName)
                            ModelState.AddModelError("FirstName", "Current value: "
                                + databaseValues.FirstName);
                        if (databaseValues.MiddleName != clientValues.MiddleName)
                            ModelState.AddModelError("MiddleName", "Current value: "
                                + databaseValues.MiddleName);
                        if (databaseValues.LastName != clientValues.LastName)
                            ModelState.AddModelError("LastName", "Current value: "
                                + databaseValues.LastName);
                        if (databaseValues.Phone != clientValues.Phone)
                            ModelState.AddModelError("Phone", "Current value: "
                                + String.Format("{0:(###) ###-####}", databaseValues.Phone));
                      
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        applicantToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException dex)
                {
                    if (dex.InnerException.InnerException.Message.Contains("IX_Unique"))
                    {
                        ModelState.AddModelError("EMail", "Unable to save changes. Remember, you cannot have duplicate eMail address.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists go for coffee.");
                    }
                }

            }
            PopulateAssignedSkillData(applicantToUpdate);
            return View(applicantToUpdate);
        }

        // GET: Applicants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Applicant applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            return View(applicant);
        }

        // POST: Applicants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Applicant applicant = db.Applicants.Find(id);
            try
            {
                db.Applicants.Remove(applicant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException dex)
            {
                if (dex.InnerException.InnerException.Message.Contains("FK_"))
                {
                    ModelState.AddModelError("", "You cannot delete a Applicant that has applicaitons in the system.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists spin around three times and spit.");
                }
            }
            return View(applicant);

        }

        private void PopulateAssignedSkillData(Applicant applicant)
        {
            var allSkills = db.Skills;
            var appSkills = new HashSet<int>(applicant.Skills.Select(b => b.ID));
            var viewModel = new List<AssignedSkillVM>();
            foreach (var sk in allSkills)
            {
                viewModel.Add(new AssignedSkillVM
                {
                    SkillID = sk.ID,
                    SkillName = sk.SkillName,
                    Assigned = appSkills.Contains(sk.ID)
                });
            }
            ViewBag.Skills = viewModel;
        }

        private void UpdateApplicantSkills(string[] selectedSkills, Applicant applicantToUpdate)
        {
            if (selectedSkills == null)
            {
                applicantToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedSkillsHS = new HashSet<string>(selectedSkills);
            var applicantSkills = new HashSet<int>
                (applicantToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected skills
            foreach (var skill in db.Skills)
            {
                if (selectedSkillsHS.Contains(skill.ID.ToString()))
                {
                    if (!applicantSkills.Contains(skill.ID))
                    {
                        applicantToUpdate.Skills.Add(skill);
                    }
                }
                else
                {
                    if (applicantSkills.Contains(skill.ID))
                    {
                        applicantToUpdate.Skills.Remove(skill);
                    }
                }
            }
        }
        private void AddPicture(ref Applicant applicant, HttpPostedFileBase f)
        {
            if (f!= null)
            {
                string mimeType = f.ContentType;
                int fileLength = f.ContentLength;
                if ((mimeType.Contains("image") && fileLength > 0))//Looks like we have a file!!!
                {
                    //Save the full size image
                    Stream fileStream = f.InputStream;
                    byte[] fullData = new byte[fileLength];
                    fileStream.Read(fullData, 0, fileLength);
                    //This is used for both Create and Edit so must decide
                    if (applicant.ApplicantImage == null)//Create New 
                    {
                        ApplicantImage fullImage = new ApplicantImage
                        {
                            imageContent = fullData,
                            imageMimeType = mimeType
                        };
                        applicant.ApplicantImage = fullImage;
                    }
                    else //Update the current image
                    {
                        applicant.ApplicantImage.imageContent = fullData;
                        applicant.ApplicantImage.imageMimeType = mimeType;
                    }
                }
            }

        }

        private void AddDocuments(ref Applicant applicant, IEnumerable<HttpPostedFileBase> docs)
        {
            foreach (var f in docs)
            {
                if (f!= null)
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

                        aFile newFile = new aFile
                        {
                            FileContent = new FileContent
                            {
                                Content = fileData,
                                MimeType = mimeType
                            },
                            fileName = fileName
                        };
                        applicant.Files.Add(newFile);
                    };
                }
            }
        }

        public FileContentResult Download(int id)
        {
            var theFile = db.Files.Include(f => f.FileContent).Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent.Content, theFile.FileContent.MimeType, theFile.fileName);
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

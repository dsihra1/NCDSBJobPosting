using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommunityProject_ProjectUnity.DAL;
using CommunityProject_ProjectUnity.Models;
using System.Data.Entity.Infrastructure;
using CommunityProject_ProjectUnity.ViewModels;

namespace CommunityProject_ProjectUnity.Controllers
{
   
    public class PositionsController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Positions
        public ActionResult Index()
        {
            PopulateDropDownLists();
            var positions = db.Positions;
                

            return View(positions.ToList());
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            var position = new Position();
            PopulateDropDownLists();
            var postype = position.PositionType;
            if (postype == "Non-Teaching")
            {
                PopulateAssignedSkillData(position);
            }
            if (postype == "Teaching")
            {
                PopulateAssignedQualificationData(position);
            }
            PopulateAssignedSkillData(position);
            PopulateCreateQualificationData(position);


            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,PositionDescription,PositionSalary,SalaryType,JobCode,PositionType,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] Position position, string[] selectedOptions, string[] selectedqualOptions)
        {

            try
            {

                if (selectedOptions != null)
                {
                    position.Skills = new List<Skill>();
                    foreach (var skill in selectedOptions)
                    {
                        var skillToAdd = db.Skills.Find(int.Parse(skill));
                        position.Skills.Add(skillToAdd);
                    }
                }


                if (selectedqualOptions != null)
                {
                    position.Qualifications = new List<Qualification>();
                    foreach (var qual in selectedqualOptions)
                    {
                        var qualToAdd = db.Qualifications.Find(int.Parse(qual));
                        position.Qualifications.Add(qualToAdd);
                    }
                }

                if (ModelState.IsValid)
                {
                    db.Positions.Add(position);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }




              
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            ViewBag.PositionID = new SelectList(db.Skills, "ID", "Name", position.ID);
            PopulateDropDownLists(position);
            PopulateAssignedQualificationData(position);
            PopulateAssignedSkillData(position);
            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            var postype = position.PositionType;
            if (postype == "Non-Teaching")
            {
                PopulateAssignedSkillData(position);
            }
            if (postype == "Teaching")
            {
                PopulateAssignedQualificationData(position);
            }
            PopulateDropDownLists();

            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, Byte[] rowVersion, string[] selectedOptions)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var positionToUpdate = db.Positions.Find(id);
            if (TryUpdateModel(positionToUpdate, "",
               new string[] { "PositionName", "PositionDescription", "PositionSalary", "JobCode","PositionType" }))
            {

                try
                {
                    var positype = db.Postings.Where(p => p.ID == id)
                                   .Select(q => q.PositionType)
                                   .FirstOrDefault();
                    if (positype == "Non-Teaching")
                    {
                        UpdatePositionSkills(selectedOptions, positionToUpdate);
                    }
                    if (positype == "Teaching")
                    {
                        UpdatePositionQualifications(selectedOptions, positionToUpdate);
                    }

                    db.Entry(positionToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)// Added for concurrency
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Position)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Position was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Position)databaseEntry.ToObject();
                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("PositionName", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.PositionDescription != clientValues.PositionDescription)
                            ModelState.AddModelError("PositionDescription", "Current value: "
                                + databaseValues.PositionDescription);
                        if (databaseValues.PositionSalary != clientValues.PositionSalary)
                            ModelState.AddModelError("PositionSalary", "Current value: "
                                + String.Format("{0:c}", databaseValues.PositionSalary));
                        
                       
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        positionToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateDropDownLists(positionToUpdate);
            var postype = db.Postings.Where(p => p.ID == id)
                                   .Select(q => q.PositionType)
                                   .FirstOrDefault();
            if (postype == "Non-Teaching")
            {
                PopulateAssignedSkillData(positionToUpdate);
            }
            if (postype == "Teaching")
            {
                PopulateAssignedQualificationData(positionToUpdate);
            }
            return View(positionToUpdate);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = db.Positions.Find(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Position position = db.Positions.Find(id);
        //    try
        //    {
        //        db.Positions.Remove(position);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch (DataException dex)
        //    {
        //        if (dex.InnerException.InnerException.Message.Contains("FK_"))
        //        {
        //            ModelState.AddModelError("", "You cannot delete a Position that has been posted.");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //        }
        //    }
        //    return View(position);
        //}
        List<SelectListItem> PositionType = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Teaching", Value = "Teaching" },
                new SelectListItem { Text = "Non-Teaching", Value = "Non-Teaching" },


            };
        private void PopulateDropDownLists(Position position = null)
        {
           
           
            ViewBag.PositionType = new SelectList(PositionType, "Value", "Text", position?.PositionType);
        }

        public JsonResult GetOptListByPositionType(string id)
        {
            var result = "";
            Position position = new Position();
           if(id=="Teaching")
            {
                PopulateAssignedQualificationData(position);
            }
           else
            {
                PopulateAssignedSkillData(position);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        private void UpdatePositionSkills(string[] selectedOptions, Position positionToUpdate)
        {
            if (selectedOptions == null)
            {
                positionToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingSkills = new HashSet<int>
                (positionToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected skills
            foreach (var skill in db.Skills)
            {
                if (selectedOptionsHS.Contains(skill.ID.ToString()))
                {
                    if (!postingSkills.Contains(skill.ID))
                    {
                        positionToUpdate.Skills.Add(skill);
                    }
                }
                else
                {
                    if (postingSkills.Contains(skill.ID))
                    {
                        positionToUpdate.Skills.Remove(skill);
                    }
                }
            }
        }

        private void UpdatePositionQualifications(string[] selectedOptions, Position positionToUpdate)
        {
            if (selectedOptions == null)
            {
                positionToUpdate.Qualifications = new List<Qualification>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingqualification = new HashSet<int>
                (positionToUpdate.Qualifications.Select(c => c.ID));//IDs of the currently selected skills
            foreach (var qual in db.Qualifications)
            {
                if (selectedOptionsHS.Contains(qual.ID.ToString()))
                {
                    if (!postingqualification.Contains(qual.ID))
                    {
                        positionToUpdate.Qualifications.Add(qual);
                    }
                }
                else
                {
                    if (postingqualification.Contains(qual.ID))
                    {
                        positionToUpdate.Qualifications.Remove(qual);
                    }
                }
            }
        }

        public JsonResult GetPositionByJobCode(string id)
        {
            List<Position> positions = new List<Position>();
            if (id != "")
            {
                positions = db.Positions.Where(p => p.JobCode.Equals(id)).ToList();
            }
            else
            {
                positions.Insert(0, new Position { JobCode = "0", Name = "Select a Job Code" });
            }
            var result = (from p in positions
                          select new
                          {
                              id = p.ID,
                              name = p.Name
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetJobCodeByPositionType(string id)
        {
            List<Position> positions = new List<Position>();
            if (id != null)
            {
                positions = db.Positions.Where(j => j.PositionType == id).ToList();
            }
            else
            {
                positions.Insert(0, new Position { PositionType = "Select a Position Type" });
            }
            var result = (from p in positions
                          select new
                          {
                              id = p.JobCode,
                              name = p.JobCode
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        private void PopulateAssignedSkillData(Position position)
        {
            var allSkills = db.Skills;
            var appSkills = new HashSet<int>(position.Skills.Select(b => b.ID));
            var viewModelAvailable = new List<AssignedSkillVM>();
            var viewModelSelected = new List<AssignedSkillVM>();
            foreach (var skills in allSkills)
            {
                if (appSkills.Contains(skills.ID))
                {
                    viewModelSelected.Add(new AssignedSkillVM
                    {
                        SkillID = skills.ID,
                        SkillName = skills.SkillName,
                        // Assigned = true 

                    });
                }
                else
                {
                    viewModelAvailable.Add(new AssignedSkillVM
                    {
                        SkillID = skills.ID,
                        SkillName = skills.SkillName,
                        // Assigned = false
                    });
                }
            }
            ViewBag.selOpts = new MultiSelectList(viewModelSelected, "SkillID", "SkillName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable, "SkillID", "SkillName");
        }
        private void PopulateAssignedQualificationData(Position position)
        {
            var allqual = db.Qualifications;
            var appquals = new HashSet<int>(position.Qualifications.Select(b => b.ID));
            var viewModelAvailable = new List<AssignedQualificationVM>();
            var viewModelSelected = new List<AssignedQualificationVM>();
            foreach (var ql in allqual)
            {
                if (appquals.Contains(ql.ID))
                {
                    viewModelSelected.Add(new AssignedQualificationVM
                    {
                        QualificationID = ql.ID,
                        QualificationName = ql.Name,
                        // Assigned = true 

                    });
                }
                else
                {
                    viewModelAvailable.Add(new AssignedQualificationVM
                    {
                        QualificationID = ql.ID,
                        QualificationName = ql.Name,
                        // Assigned = true 

                    });
                }
            }
            ViewBag.selOpts = new MultiSelectList(viewModelSelected, "QualificationID", " QualificationName");
            ViewBag.availOpts = new MultiSelectList(viewModelAvailable, "QualificationID", " QualificationName");
        }

        private void PopulateCreateQualificationData(Position position)
        {
            var allqual = db.Qualifications;
            var appquals = new HashSet<int>(position.Qualifications.Select(b => b.ID));
            var viewModelAvailable = new List<AssignedQualificationVM>();
            var viewModelSelected = new List<AssignedQualificationVM>();
            foreach (var ql in allqual)
            {
                if (appquals.Contains(ql.ID))
                {
                    viewModelSelected.Add(new AssignedQualificationVM
                    {
                        QualificationID = ql.ID,
                        QualificationName = ql.Name,
                        // Assigned = true 

                    });
                }
                else
                {
                    viewModelAvailable.Add(new AssignedQualificationVM
                    {
                        QualificationID = ql.ID,
                        QualificationName = ql.Name,
                        // Assigned = true 

                    });
                }
            }
            ViewBag.selqualOpts = new MultiSelectList(viewModelSelected, "QualificationID", " QualificationName");
            ViewBag.availqualOpts = new MultiSelectList(viewModelAvailable, "QualificationID", " QualificationName");
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

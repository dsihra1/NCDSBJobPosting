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

namespace CommunityProject_ProjectUnity.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Humanresources")]
    public class FilesController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Files
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Humanresources")]
        public ActionResult Index()
        {
            var Files = db.Files.Include(a => a.Applicant).Include(a => a.FileContent);
            return View(Files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            aFile aFile = db.Files.Find(id);
            if (aFile == null)
            {
                return HttpNotFound();
            }
            return View(aFile);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName");
            ViewBag.ID = new SelectList(db.FileContents, "FileContentID", "MimeType");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,fileName,Description,ApplicantID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] aFile aFile)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(aFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", aFile.ApplicantID);
            ViewBag.ID = new SelectList(db.FileContents, "FileContentID", "MimeType", aFile.ID);
            return View(aFile);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            aFile aFile = db.Files.Find(id);
            if (aFile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", aFile.ApplicantID);
            ViewBag.ID = new SelectList(db.FileContents, "FileContentID", "MimeType", aFile.ID);
            return View(aFile);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,fileName,Description,ApplicantID,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion")] aFile aFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantID = new SelectList(db.Applicants, "ID", "FirstName", aFile.ApplicantID);
            ViewBag.ID = new SelectList(db.FileContents, "FileContentID", "MimeType", aFile.ID);
            return View(aFile);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            aFile aFile = db.Files.Find(id);
            if (aFile == null)
            {
                return HttpNotFound();
            }
            return View(aFile);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            aFile aFile = db.Files.Find(id);
            db.Files.Remove(aFile);
            db.SaveChanges();
            return RedirectToAction("Index");
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

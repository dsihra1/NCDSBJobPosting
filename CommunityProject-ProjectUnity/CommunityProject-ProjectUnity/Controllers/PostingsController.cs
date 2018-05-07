using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using CommunityProject_ProjectUnity.DAL;
using CommunityProject_ProjectUnity.Models;
using CommunityProject_ProjectUnity.ViewModels;
using PagedList;

namespace CommunityProject_ProjectUnity.Controllers
{
    [Authorize]
    public class PostingsController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Postings
        public ActionResult Index(string sortDirection, string sortField,
            string actionButton, string searchName,  string searchStatus, string SalaryType, string School, string searchFTE, string[] selectedOptions, int? page)
        {
            PopulateDropDownLists();
            ViewBag.School= new SelectList(db.Schools.OrderBy(p => p.Name), "Name", "Name");
            ViewBag.Filtering = ""; //Assume not filtering

            //Prepare a ghost Posting to help maintain the selected skills
            var posting = new Posting();
            posting.Skills = new List<Skill>();
            posting.Qualifications = new List<Qualification>();

            //LINQ - Start with the Includes
            var postings = db.Postings.AsNoTracking()
                .Include(p => p.Position)
                .Include(p=>p.Schools)
               
                .Include(p => p.Skills);
                
            if (!User.IsInRole("Admin"))
            {
                postings = postings.Where(p => p.status == "Current");
            }



            //Add as many filters as you want
            if (!String.IsNullOrEmpty(searchName))
            {
                postings = postings.Where(p => p.Name.ToUpper().Contains(searchName.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchName = searchName;
            }
           
            if (!String.IsNullOrEmpty(searchStatus))
            {
                postings = postings.Where(p => p.status.ToUpper().Contains(searchStatus.ToUpper()));
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.searchStatus = searchStatus;
            }


            if (!String.IsNullOrEmpty(SalaryType))
            {
                if(SalaryType=="1")
                {
                    postings = postings.Where(p => p.SalaryType=="Hourly");
                }
                if(SalaryType=="2")
                {
                    postings = postings.Where(p => p.SalaryType == "Annually");
                }
                
                ViewBag.Filtering = " in";//Flag filtering
                ViewBag.SalaryType = SalaryType;
            }

            if (!String.IsNullOrEmpty(searchFTE))
            {
                if (searchFTE == "1")
                {
                    postings = postings.Where(p => p.FTE < 0.5);
                    ViewBag.Filtering = " in";//Flag filtering
                    ViewBag.searchName = searchFTE;
                }
                else
                {
                    postings = postings.Where(p => p.FTE >= 0.5);
                    ViewBag.Filtering = " in";//Flag filtering
                    ViewBag.searchName = searchFTE;
                }
            }

            //Before we sort, see if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                //Reset paging if ANY button was pushed
                page = 1;

                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }
            //Now we know which field and direction to sort by, but a Switch is hard to use for 2 criteria
            //so we will use an if() structure instead.
            if (sortField == "Number of Openings")//Sorting by Applicant Name
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.Openings);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.Openings);
                }
            }
            else if (sortField == "Closing Date")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.ClosingDate);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.ClosingDate);
                }
            }
            else if (sortField == "Full Time Equivalent")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.FTE);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.FTE);
                }
            }
            else //By default sort by Position NAME
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    postings = postings
                        .OrderBy(p => p.Position.Name);
                }
                else
                {
                    postings = postings
                        .OrderByDescending(p => p.Position.Name);
                }
            }
            ViewBag.searchStatus = new SelectList(Status, "Value", "Text", posting?.status);
            
           
            //Prepare the check boxes
            PopulateAssignedSkillData(posting);
            PopulateAssignedQualificationData(posting);

            //Set sort for next time
            ViewBag.sortField = sortField;
            ViewBag.sortDirection = sortDirection;

            int pageSize = 550;
            int pageNumber = (page ?? 1);
            if (!User.IsInRole("Admin"))
            {
                return View("Indexuser", postings.Where(p =>p.status=="Current")
                    .ToPagedList(pageNumber, pageSize));
            }
            return View(postings.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult UserView()
        {
            return View(db.Postings.ToList());
        }
        //public ActionResult Index()
        //{
        //    if (!User.IsInRole("Admin"))
        //    {
        //        return View("Indexuser", db.Postings.OrderBy(p => p.Name).ToPagedList(1, 25));
        //    }

        //    return View(db.Postings.OrderBy(p => p.Name).ToPagedList(1, 25));

        //}
        // GET: Postings/Details/5
        public ActionResult Details(int? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.AsNoTracking()
                //.Include(p  =>p.City)
                //.Include(p => p.School)
                .Include(p => p.Skills)
                .Include(p =>p.Qualifications)
                .Include(p => p.Position)
                .Include(p =>p.Applications)
                .Where(p => p.ID == id).SingleOrDefault();
            var postype = posting.PositionType;
            if(postype=="Teaching")
            {
                ViewBag.SkillsList = posting.Qualifications.ToList();
            }
            if(postype=="Non-Teaching")
            {
                ViewBag.SkillsList = posting.Skills.ToList();
            }
            if (posting == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = message;
            ViewBag.Closed = posting.ClosingDate < DateTime.Today;
            return View(posting);
        }

        // GET: Postings/Create
        
        public ActionResult CreateStart()
        {
            PopulateDropDownLists();
            return View("CreateStart");
        }

        // GET: Postings/Create
        //public ActionResult Create()
        //{
        //    PopulateDropDownLists();
        //    //Add all (unchecked) Skills to ViewBag
        //    var posting = new Posting();
        //    posting.Skills = new List<Skill>();
        //    posting.Qualifications = new List<Qualification>();
        //    var result = (from skills in db.Skills select skills).ToList();
        //    var qResult = (from qualifications in db.Qualifications select qualifications).ToList();
        //    if (result != null)
        //    {
        //        ViewBag.mySkills = result.Select(N => new SelectListItem { Text = N.SkillName, Value = N.ID.ToString() });
        //    }
        //    if (result != null)
        //    {
        //        ViewBag.myQualifications = qResult.Select(N => new SelectListItem { Text = N.Name, Value = N.ID.ToString() });
        //    }
        //    PopulateAssignedSkillData(posting);
        //    PopulateAssignedQualificationData(posting);
        //    return View();
        //}
        public ActionResult Create(int? PositionID)
        {
            Position position = db.Positions
                .Include(p => p.Skills)
                .Where(p => p.ID == PositionID)
               
                .SingleOrDefault();
            if (position == null)
            {
                ModelState.AddModelError("", "No Position to use as a Template");
                PopulateDropDownLists();
                return View("CreateStart");
            }
            //We have the positon to use as a template
            var posting = new Posting()
            {
                PositionID = position.ID,
                Position = position,
                Openings = 1,
                status="Current",
                FTE=0.5,
                Name=position.Name,
                Description = position.PositionDescription,
                Salary = position.PositionSalary,
                Skills = position.Skills,
                PositionType = position.PositionType,
                Qualifications = position.Qualifications,
                SalaryType = "Annually"
            };
            ViewBag.SkillsList = posting.Skills.ToList();

            var postype = posting.PositionType;
            if (postype == "Non-Teaching")
            {
                PopulateAssignedSkillData(posting);
            }
            if (postype == "Teaching")
            {
                PopulateAssignedQualificationData(posting);
            }
           
            PopulateAssignedSchoolData(posting);
            PopulateDropDownLists();
            return View("Create", posting);
        }

        

        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "ID,Openings,Name,ClosingDate,StartDate,JobStartDate,JobEndDate,JobTypeID,PositionType,Salary,SalaryType,Description,CityID,SchoolID,FTE,status,PositionID")] Posting posting, string[] selectedOptions, string[] selectedCities,string [] selectedSchools)
        {
            try
            {
                //Add the selected skills/Qualifications
                
                if (selectedOptions != null)
                {
                    posting.Skills = new List<Skill>();
                    foreach (var skill in selectedOptions)
                    {
                        var skillToAdd = db.Skills.Find(int.Parse(skill));
                        posting.Skills.Add(skillToAdd);
                    }
                    posting.Qualifications = new List<Qualification>();
                    foreach (var qual in selectedOptions)
                    {
                        var qualToAdd = db.Qualifications.Find(int.Parse(qual));
                        posting.Qualifications.Add(qualToAdd);
                    }

                }
                if(selectedCities !=null)
                {
                  
                    foreach(var city in selectedCities)
                    {
                        
                    }
                }
                if(selectedSchools !=null)
                {
                    posting.Schools = new List<School>();
                    foreach(var school in selectedSchools)
                    {
                        var SchoolToAdd = db.Schools.Find(int.Parse(school));
                        posting.Schools.Add(SchoolToAdd);
                    }
                }

                
                if (ModelState.IsValid)
                {
                    db.Postings.Add(posting);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            //catch (DataException)
            //{
            //    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            //}

            PopulateDropDownLists(posting);
           
            PopulateAssignedSchoolData(posting);
            PopulateAssignedSkillData(posting);
            PopulateAssignedQualificationData(posting);
            return View(posting);
        }

        // GET: Postings/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings
                .Include(p => p.Skills)
                .Include(p =>p.Qualifications)
                
                .Where(p => p.ID == id).SingleOrDefault();
            
            if (posting == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                if (posting.ClosingDate < DateTime.Today)
                {
                    return RedirectToAction("Details", new { id = posting.ID });
                }
            }
            var postype = db.Postings.Where(p => p.ID == id)
                                    .Select(q => q.PositionType)
                                    .FirstOrDefault();
            

            if (postype == "Non-Teaching")
            {
                PopulateAssignedSkillData(posting);
            }
            if (postype == "Teaching")
            {
                PopulateAssignedQualificationData(posting);
            }
           
            PopulateAssignedSchoolData(posting);
            PopulateDropDownLists(posting);
           
            
            return View(posting);
        }

        


        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
       
        public ActionResult EditPost(int? id, Byte[] rowVersion, string[] selectedOptions, string[] selectedCities,string[] selectedSchools)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postingToUpdate = db.Postings
                .Include(p => p.Skills)
                .Include(p => p.Qualifications)
                
                .Include(p =>p.Schools)
                .Where(p => p.ID == id).SingleOrDefault();
            
            if (TryUpdateModel(postingToUpdate, "",
               new string[] { "Openings", "ClosingDate", "JobStartDate", "JobEndDate", "StartDate", "PositionID","Name","status", "CityID", "JobCode","FTE", "Salary","SalaryType", "Description", "SchoolID" }))
            {
                try
                {
                    var postype = db.Postings.Where(p => p.ID == id)
                                    .Select(q => q.PositionType)
                                    .FirstOrDefault();
                    if(postype== "Non-Teaching")
                    {
                        UpdatePostingSkills(selectedOptions, postingToUpdate);
                    }
                    if (postype == "Teaching")
                    {
                        UpdatePostingQualifications(selectedOptions, postingToUpdate);
                    }

                    
                    UpdatePostingSchools(selectedSchools, postingToUpdate);
                    

                    db.Entry(postingToUpdate).OriginalValues["RowVersion"] = rowVersion;
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
                    var clientValues = (Posting)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Posting was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Posting)databaseEntry.ToObject();
                        if (databaseValues.ClosingDate != clientValues.ClosingDate)
                            ModelState.AddModelError("ClosingDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.ClosingDate));
                        if (databaseValues.StartDate != clientValues.StartDate)
                            ModelState.AddModelError("StartDate", "Current value: "
                                + String.Format("{0:d}", databaseValues.StartDate));
                        if (databaseValues.Openings != clientValues.Openings)
                            ModelState.AddModelError("Openings", "Current value: "
                                + databaseValues.Openings);
                        if (databaseValues.PositionID != clientValues.PositionID)
                            ModelState.AddModelError("PositionID", "Current value: "
                                + db.Positions.Find(databaseValues.PositionID).Name);
                        if (databaseValues.status != clientValues.status)
                            ModelState.AddModelError("status", "Current value: "
                                + databaseValues.status);
                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        //if (databaseValues.CityID != clientValues.CityID)
                        //    ModelState.AddModelError("City", "Current value: "
                        //        + databaseValues.City);
                       
                        if (databaseValues.FTE != clientValues.FTE)
                            ModelState.AddModelError("FTE", "Current value: "
                                + databaseValues.FTE);
                        if (databaseValues.Salary != clientValues.Salary)
                            ModelState.AddModelError("Salary", "Current value: "
                                + databaseValues.Salary);
                        if (databaseValues.Openings != clientValues.Openings)
                            ModelState.AddModelError("Openings", "Current value: "
                                + databaseValues.Description);

                        //if (databaseValues.SchoolID != clientValues.SchoolID)
                        //    ModelState.AddModelError("SchoolName", "Current value: "
                        //        + databaseValues.School.Name);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        postingToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (DataException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateDropDownLists(postingToUpdate);
            PopulateAssignedSkillData(postingToUpdate);
            PopulateAssignedQualificationData(postingToUpdate);
            PopulateAssignedSchoolData(postingToUpdate);
            
            return View(postingToUpdate);
        }

       



        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posting posting = db.Postings
                
                .Where(d => d.ID == id)
                .SingleOrDefault();


            db.Postings.Remove(posting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CSVUpload()
        {
            return View();
        }
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CSVUpload(HttpPostedFileBase FileUpload)
        {
            // Set up DataTable place holder
            // Set up DataTable place holder
            DataTable dt = new DataTable();

            //check we have a file
            if (FileUpload.ContentLength > 0)
            {
                //Workout our file path
                string fileName = Path.GetFileName(FileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

                //Try and upload
                try
                {
                    FileUpload.SaveAs(path);
                    //Process the CSV file and capture the results to our DataTable place holder
                    dt = ProcessCSV(path);
                    ViewBag.rowcount = dt.Rows.Count;
                    for (int i=0;i<dt.Rows.Count; i++)
                    {
                        DateTime posstartdate = DateTime.Parse(dt.Rows[i][1].ToString());
                        DateTime posenddate = DateTime.Parse(dt.Rows[i][2].ToString());
                        if(dt.Rows[i][10].ToString()!="")
                        {
                            DateTime jobstartdate = DateTime.Parse(dt.Rows[i][10].ToString());
                        }
                        else if(dt.Rows[i][11].ToString()!="")
                        {
                            DateTime jobenddate = DateTime.Parse(dt.Rows[i][11].ToString());
                        }
                       else
                        {
                            dt.Rows[i][10] = DateTime.Parse("1900-01-01");
                            dt.Rows[i][11] =DateTime.Parse("1900-01-01");
                        }
                       
                       
                       
                        dt.Rows[i][1] = posstartdate;
                        dt.Rows[i][2] = posenddate;


                       





                    }

                   


                    ////Process the DataTable and capture the results to our SQL Bulk copy
                    ViewBag.Feedback = ProcessBulkCopy(dt);
                }
                catch (Exception ex)
                {
                    //Catch errors
                    ViewBag.Feedback = ex.Message;
                }
            }
            else
            {
                //Catch errors
                ViewBag.Feedback = "Please select a file";
            }

            //Tidy up
            dt.Dispose();



            return View("CSVUpload", ViewData["Feedback"]);
        }

        public static DataTable ProcessCSV(string fileName)
        {
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();
                dt.Columns[0].ColumnName = "Name";
                dt.Columns[1].ColumnName = "StartDate";
                dt.Columns[2].ColumnName = "ClosingDate";
                dt.Columns[3].ColumnName = "FTE";
                dt.Columns[4].ColumnName = "Openings";
                dt.Columns[5].ColumnName = "Salary";
                dt.Columns[6].ColumnName = "Description";
                dt.Columns[7].ColumnName = "PositionID";
                dt.Columns[8].ColumnName = "status";
                dt.Columns[9].ColumnName = "PositionType";
                dt.Columns[10].ColumnName = "JobStartDate";
                dt.Columns[11].ColumnName = "JobEndDate";
                dt.Columns[12].ColumnName = "SalaryType";
                
                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;

        }

        public static String ProcessBulkCopy(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["ProjectUnityEntities"].ConnectionString;
           
            //make our connection and dispose at the end
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //Open our connection
                conn.Open();
                
                //Begin our transaction
                SqlTransaction trans = conn.BeginTransaction();
                //make our command and dispose at the end
                using (var copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers,
                    trans))
                {

                   
                   

                    

                    ///Set target table and tell the number of rows
                    copy.DestinationTableName = "Posting";
                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        //Send it to the server
                        copy.ColumnMappings.Add("Name", "Name");
                        copy.ColumnMappings.Add("StartDate", "StartDate");
                        copy.ColumnMappings.Add("ClosingDate", "ClosingDate");
                        copy.ColumnMappings.Add("FTE", "FTE");
                        copy.ColumnMappings.Add("Openings", "Openings");
                        copy.ColumnMappings.Add("Salary", "Salary");
                        copy.ColumnMappings.Add("Description", "Description");
                        copy.ColumnMappings.Add("PositionID", "PositionID");
                       
                        copy.ColumnMappings.Add("status", "status");
                       
                        copy.ColumnMappings.Add("PositionType", "PositionType");
                        copy.ColumnMappings.Add("JobStartDate", "JobStartDate");
                        copy.ColumnMappings.Add("JobEndDate", "JobEndDate");
                        copy.ColumnMappings.Add("SalaryType", "SalaryType");

                        copy.WriteToServer(dt);
                        trans.Commit();
                        Feedback = "Upload complete";

                    }
                    catch (Exception ex)
                    {
                        Feedback = ex.Message;
                    }

                    
                }
            }

            return Feedback;
        }

        List<SelectListItem> Status = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Current", Value = "Current" },
                new SelectListItem { Text = "Archived", Value = "Archived" },
                new SelectListItem { Text = "Closed", Value = "Closed" },
               

            };
        List<SelectListItem> SalaryType = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Annually", Value = "Annually" },
                new SelectListItem { Text = "Hourly", Value = "Hourly" },
                


            };
        List<SelectListItem> PositionType = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Teaching", Value = "Teaching" },
                new SelectListItem { Text = "Non-Teaching", Value = "Non-Teaching" },


            };
        private void PopulateDropDownLists(Posting posting = null)
        {
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(p => p.Name), "ID", "Name", posting?.PositionID);
            //ViewBag.CityID = new SelectList(db.Cities.OrderBy(p => p.Name), "ID", "Name", posting?.CityID);
            //ViewBag.SchoolID = new SelectList(db.Schools.OrderBy(p => p.Name), "ID", "Name", posting?.SchoolID);
            ViewBag.JobTypeID = new SelectList(db.Positions.OrderBy(p => p.Name), "JobCode", "JobCode");
            ViewBag.status = new SelectList(Status,"Value","Text", posting?.status);
            ViewBag.SalaryType = new SelectList(SalaryType, "Value", "Text", posting?.SalaryType);

            ViewBag.PositionType = new SelectList(PositionType, "Value", "Text", posting?.PositionType);


        }

        private void PopulateAssignedSkillData(Posting posting)
        {
            var allSkills = db.Skills;
            var appSkills = new HashSet<int>(posting.Skills.Select(b => b.ID));
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

        private void PopulateAssignedQualificationData(Posting posting)
        {
            var allqual = db.Qualifications;
            var appquals = new HashSet<int>(posting.Qualifications.Select(b => b.ID));
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

        //private void PopulateAssignedCityData(Posting posting)
        //{
            
        //    var appcities = new HashSet<int>(posting.Cities.Select(b => b.ID));
        //    var viewModelAvailable = new List<AssignedCityVM>();
        //    var viewModelSelected = new List<AssignedCityVM>();

        //    foreach (var ql in allcities)
        //    {
        //        if (appcities.Contains(ql.ID))
        //        {
        //            viewModelSelected.Add(new AssignedCityVM
        //            {
        //                CityID = ql.ID,
        //                CityName = ql.Name,
        //                // Assigned = true 

        //            });
        //        }
        //        else
        //        {
        //            viewModelAvailable.Add(new AssignedCityVM
        //            {
        //                CityID = ql.ID,
        //                CityName = ql.Name,
        //                // Assigned = true 

        //            });
        //        }
        //    }
        //    ViewBag.selCities = new MultiSelectList(viewModelSelected, "CityID", "CityName");
        //    ViewBag.availCities = new MultiSelectList(viewModelAvailable, "CityID", " CityName");
        //}


        private void PopulateAssignedSchoolData(Posting posting)
        {
            var allschools = db.Schools;
            var appschools = new HashSet<int>(posting.Schools.Select(b => b.ID));
            var viewModelAvailable = new List<AssignedSchoolVM>();
            var viewModelSelected = new List<AssignedSchoolVM>();

            foreach (var ql in allschools)
            {
                if (appschools.Contains(ql.ID))
                {
                    viewModelSelected.Add(new AssignedSchoolVM
                    {
                        SchoolID = ql.ID,
                        SchoolName = ql.Name,
                        // Assigned = true 

                    });
                }
                else
                {
                    viewModelAvailable.Add(new AssignedSchoolVM
                    {
                        SchoolID = ql.ID,
                        SchoolName = ql.Name,
                        // Assigned = true 

                    });
                }
            }
            ViewBag.selSchools = new MultiSelectList(viewModelSelected, "SchoolID", "SchoolName");
            ViewBag.availSchools = new MultiSelectList(viewModelAvailable, "SchoolID", "SchoolName");


        }
        //public ActionResult SelectCity()
        //{
        //    List<City> cities = db.Cities.ToList();
        //    cities.Insert(0, new City { ID = 0, Name = "Select a City" });

        //    List<School> schools = new List<School>();
        //    ViewBag.CityID = new SelectList(cities, "ID", "Name");
        //    //ViewBag.SchoolID = new SelectList(schools, "ID", "Name");
        //    return View();

        //}

        //public JsonResult GetSchoolByCityID(int id)
        //{
        //    List<School> schools = new List<School>();

        //    if (id > 0)
        //    {
        //        schools = db.Schools.Where(p => p.cityID == id).ToList();
        //    }
        //    else
        //    {
        //        schools.Insert(0, new School { cityID = 0, Name = "Select a City" });
        //    }
        //    var result = (from r in schools
        //                  select new
        //                  {
        //                      id = r.ID,
        //                      name = r.Name
        //                  }).ToList();

        //    return Json(result, JsonRequestBehavior.AllowGet);

        //}
        public JsonResult GetPositionByJobCode(string id)
        {
            List<Position> positions = new List<Position>();
            if (id!="")
            {
                positions = db.Positions.Where(p => p.JobCode.Equals(id)).ToList();
            }
            else
            {
                positions.Insert(0, new Position {JobCode = "0", Name = "Select a Job Code" });
            }
            var result = (from p in positions
                          select new
                          {
                              id = p.ID,
                              name = p.Name
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
  
        private void UpdatePostingSkills(string[] selectedOptions, Posting postingToUpdate)
        {
            if (selectedOptions == null)
            {
                postingToUpdate.Skills = new List<Skill>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingSkills = new HashSet<int>
                (postingToUpdate.Skills.Select(c => c.ID));//IDs of the currently selected skills
            foreach (var skill in db.Skills)
            {
                if (selectedOptionsHS.Contains(skill.ID.ToString()))
                {
                    if (!postingSkills.Contains(skill.ID))
                    {
                        postingToUpdate.Skills.Add(skill);
                    }
                }
                else
                {
                    if (postingSkills.Contains(skill.ID))
                    {
                        postingToUpdate.Skills.Remove(skill);
                    }
                }
            }
        }
        private void UpdatePostingQualifications(string[] selectedOptions, Posting postingToUpdate)
        {
            if (selectedOptions == null)
            {
                postingToUpdate.Qualifications = new List<Qualification>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var postingqualification = new HashSet<int>
                (postingToUpdate.Qualifications.Select(c => c.ID));//IDs of the currently selected skills
            foreach (var qual in db.Qualifications)
            {
                if (selectedOptionsHS.Contains(qual.ID.ToString()))
                {
                    if (!postingqualification.Contains(qual.ID))
                    {
                        postingToUpdate.Qualifications.Add(qual);
                    }
                }
                else
                {
                    if (postingqualification.Contains(qual.ID))
                    {
                        postingToUpdate.Qualifications.Remove(qual);
                    }
                }
            }
        }

        //private void UpdatePostingCities(string[] selectedCities, Posting postingToUpdate)
        //{
        //    if(selectedCities==null)
        //    {
        //        postingToUpdate.Cities = new List<City>();
        //        return;
        //    }
        //    var selectedcitiesHS = new HashSet<string>(selectedCities);
        //    var postingcities = new HashSet<int>
        //        (postingToUpdate.Cities.Select(c => c.ID));

        //    foreach (var qual in db.Cities)
        //    {
        //        if (selectedcitiesHS.Contains(qual.ID.ToString()))
        //        {
        //            if (!postingcities.Contains(qual.ID))
        //            {
        //                postingToUpdate.Cities.Add(qual);
        //            }
        //        }
        //        else
        //        {
        //            if (postingcities.Contains(qual.ID))
        //            {
        //                postingToUpdate.Cities.Remove(qual);
        //            }
        //        }
        //    }
        //}

        private void UpdatePostingSchools(string[] selectedSchools, Posting postingToUpdate)
        {

            if (selectedSchools == null)
            {
                postingToUpdate.Schools = new List<School>();
                return;
            }

            var selectedschoolsHS = new HashSet<string>(selectedSchools);
            var postingschools = new HashSet<int>
                (postingToUpdate.Schools.Select(c => c.ID));

            foreach (var qual in db.Schools)
            {
                if (selectedschoolsHS.Contains(qual.ID.ToString()))
                {
                    if (!postingschools.Contains(qual.ID))
                    {
                        postingToUpdate.Schools.Add(qual);
                    }
                }
                else
                {
                    if (postingschools.Contains(qual.ID))
                    {
                        postingToUpdate.Schools.Remove(qual);
                    }
                }
            }

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
                              id = p.ID,
                              name = p.JobCode
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetAPosition(int? ID)
        {
            try
            {
                Position position = db.Positions
                    
                    .Include(d => d.Skills)
                    .Include(d =>d.Qualifications)
                    .Where(d => d.ID == ID)
                    .SingleOrDefault();
                //Build a string of html for the skills collection
                string Skills = "";
                if (position.PositionType=="Non-Teaching")
                {
                    foreach (var s in position.Skills)
                    {
                        Skills += s.SkillName + "<br />";
                    }
                }
                else
                {

                    foreach (var s in position.Qualifications)
                    {
                        Skills += s.Name + "<br />";
                    }

                }
                
               
                //Using an annomous object as a DTO to avoid serialization errors
                var pos = new
                {
                    position.Name,
                    position.PositionDescription,
                    Salary = position.PositionSalary.ToString("c"),
                    Assignedskill = Skills
                };
                return Json(pos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return HttpNotFound();
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

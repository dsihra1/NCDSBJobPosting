using CommunityProject_ProjectUnity.DAL;
using CommunityProject_ProjectUnity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;

namespace CommunityProject_ProjectUnity.Controllers
{
    public class HomeController : Controller
    {
        private ProjectUnityEntities db = new ProjectUnityEntities();

        // GET: Postings
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Postings");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
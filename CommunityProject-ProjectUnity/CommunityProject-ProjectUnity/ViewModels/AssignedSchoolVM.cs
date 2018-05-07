using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.ViewModels
{
    public class AssignedSchoolVM
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public bool Assigned { get; set; }
    }
}
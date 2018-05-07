using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.ViewModels
{
    public class AssignedUserFilesVM
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public bool Assigned { get; set; }
    }
}
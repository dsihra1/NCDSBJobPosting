using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.ViewModels
{
    public class AssignedCityVM
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public bool Assigned { get; set; }
    }
}
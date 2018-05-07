using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class aFile : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "File Name")]
        [StringLength(256)]
        public string fileName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual FileContent FileContent { get; set; }

        public int ApplicantID { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}
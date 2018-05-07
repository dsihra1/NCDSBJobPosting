using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class ApplicationFiles:Auditable
    {
        public int ID { get; set; }

        [Display(Name = "File Name")]
        [StringLength(256)]
        public string fileName { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual ApplicationFilesContent FilesContent { get; set; }

        public int ApplicationID { get; set;}

        public virtual Application Application { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class ApplicantImage
    {
        [Key, ForeignKey("Applicant")]
        public int ApplicantImageID { get; set; }
        [ScaffoldColumn(false)]
        public byte[] imageContent { get; set; }

        [StringLength(256)]
        [ScaffoldColumn(false)]
        public string imageMimeType { get; set; }

        public virtual Applicant Applicant { get; set; }
    }
}
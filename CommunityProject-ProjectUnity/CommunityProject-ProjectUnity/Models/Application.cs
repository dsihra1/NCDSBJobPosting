using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class Application : Auditable
    {
        public Application()
        {
            this.Files = new HashSet<ApplicationFiles>();
            this.UserFiles = new HashSet<aFile>();
        }
       
           
        public int ID { get; set; }

        [Display(Name = "Covering Comments")]
        [Required(ErrorMessage = "You cannot leave the summary comments blank.")]
        [StringLength(2000, ErrorMessage = "Summary must be between 20 and 2000 characters", MinimumLength = 20)]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

        [Required(ErrorMessage = "You must specify the job posting applied for.")]
        [Index("IX_Unique_Application", Order = 1, IsUnique = true)]
        public int PostingID { get; set; }

        [Required(ErrorMessage = "You must specify the applicant applying to the job posting.")]
        [Index("IX_Unique_Application", Order = 2)]
        public int ApplicantID { get; set; }

       
        public virtual ICollection<ApplicationFiles> Files { get; set; }

        public virtual ICollection<aFile> UserFiles { get; set; }

        public virtual Posting Posting { get; set; }

        [Display(Name = "Application Status")]
        public string Applicationstatus { get; set; }
        

        public virtual Applicant Applicant { get; set; }
    }
}
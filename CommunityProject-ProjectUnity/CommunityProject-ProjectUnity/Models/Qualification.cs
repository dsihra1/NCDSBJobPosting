using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class Qualification : Auditable
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Qualification Name Required")]
        [Display(Name = "Qualification")]
        [StringLength(50, ErrorMessage = "Qualification Must Be Shorter Than 50 Characters")]
        [Index("IX_Unique_Qualification", IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Qualification Description Required")]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Qualification Must Be Shorter Than 500 Characters")]
        public string Description { get; set; }

        

        public virtual ICollection<Applicant> Applicants { get; set; }

        public virtual ICollection<Posting> Postings { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}
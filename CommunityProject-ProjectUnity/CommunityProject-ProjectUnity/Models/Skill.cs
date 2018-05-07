using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class Skill:Auditable
    {
        public int ID { get; set; }

        [Display(Name = "Skill")]
        [Required(ErrorMessage = "Skill set is required")]
        [StringLength(50, ErrorMessage = "Too many characters")]
        [Index("IX_Unique_Skill", IsUnique = true)]
        public string SkillName { get; set; }


        [Required(ErrorMessage = "Skill Description Required")]
        [Display(Name = "Qualification")]
        [StringLength(500, ErrorMessage = "Skill Description Must Be Shorter Than 500 Characters")]
        public string Description { get; set; }

        public virtual ICollection<Applicant> Applicants { get; set; }

        public virtual ICollection<Posting> Postings { get; set; }

        public virtual ICollection<Position> Positions { get; set; }
    }
}
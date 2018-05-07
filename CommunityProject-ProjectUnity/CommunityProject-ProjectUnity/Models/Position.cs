using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class Position : Auditable
    {
        public Position()
        {
            this.Postings = new HashSet<Posting>();
            this.Skills = new HashSet<Skill>();
            this.Qualifications = new HashSet<Qualification>();

        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Position Name Required")]
        [Display(Name = "Position Name")]
        [StringLength(50, ErrorMessage = "Position Name Must Be Shorter Than 50 Characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Position Description Required")]
        [Display(Name = "Description")]
        [StringLength(1200, ErrorMessage = "Position Description Must Be Shorter Than 1200 Characters")]
        public string PositionDescription { get; set; }


        [Required(ErrorMessage = "Salary Type Required")]
        [Display(Name = "Salary Type")]
        public string SalaryType { get; set; }

        [Required(ErrorMessage = "Salary Required")]
        [Display(Name = "Salary")]
        [Range(0.01, 999999.99, ErrorMessage = "Position Salary must be between $0.01 and $999999.99")]
        [DataType(DataType.Currency)]
        public int PositionSalary { get; set; }

        [Required(ErrorMessage = "Job Code is Required")]
        [Display(Name = "Job Code")]
        [StringLength(10, ErrorMessage = "Job Code cannot be more than  10 Characters")]
        [Index("IX_Unique_JobCode", IsUnique = true)]
        public string JobCode { get; set; }

        [Required(ErrorMessage = "Position Type is Required")]
        public string PositionType { get; set; }

        public virtual ICollection<Qualification> Qualifications { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        [Display(Name = "Position")]
        public string JobCodeName
        {
            get
            {
                return JobCode + "-" + Name;
            }
            
        }


        public virtual ICollection<Posting> Postings { get; set; }
    }
}
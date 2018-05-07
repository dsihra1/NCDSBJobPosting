using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
    public class Posting : Auditable, IValidatableObject
    {
        public Posting()
        {
            this.Applications = new HashSet<Application>();
            this.Skills = new HashSet<Skill>();
            this.Qualifications = new HashSet<Qualification>();
            this.Schools = new HashSet<School>();
           
        }
        public int ID { get; set; }



        [Display(Name = "Posting")]
        public string Summary
        {
            get
            {
                return Position?.Name;
            }
        }

        [Display(Name = "Posting")]
        public string ClosingSummary
        {
            get
            {
                string tense = (ClosingDate < DateTime.Today) ? "Closed: " : "Closing: ";
                return tense + ClosingDate.ToShortDateString();
            }
        }

        [Display(Name = "Posting")]
        public string FullSummary
        {
            get
            {
                return Position?.Name + " - " + ClosingSummary;
            }
        }

        [Required(ErrorMessage = "Posting Name is Required")]
        [Display(Name = "Job Posting")]
        [StringLength(150, ErrorMessage = "Posting Name Must Be Shorter Than 150 Characters")]
        public string Name { get; set; }


        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }


        [Display(Name = "Closing Date")]
        [Required(ErrorMessage = "You must specify the closing date for the job posting.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }


        [Display(Name = "Job Start Date")]
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? JobStartDate { get; set; }

        [Display(Name = "Job End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? JobEndDate { get; set; }

        [Display(Name = "Posting Status")]
        [Required(ErrorMessage = "You must provide a posting status.")]
        public string status { get; set; }

       

        [Display(Name = "Full Time Equivalent")]
        [Required(ErrorMessage = "You must specify the F.T.E for the job posting.")]
        [Range(typeof(double), "0.1", "1.2", ErrorMessage = "FTE has to be between 0.1 and 1.2")]
        public double? FTE { get; set; }

        [Display(Name = "Number of Openings")]
        [Required(ErrorMessage = "You must specify how many positions are open.")]
        [Range(1, 9999, ErrorMessage = "Invalid number of job openings.")]
        public int Openings { get; set; }

        [Required(ErrorMessage = "Salary Required")]
        [Display(Name = "Salary")]
        [Range(0.01, 999999.99, ErrorMessage = "Salary must be between $0.01 and $999999.99")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Salary Type Required")]
        [Display(Name = "Salary Type")]
        public string SalaryType { get; set; }


        [Required(ErrorMessage = "Posting Description Required")]
        [Display(Name = "Posting Description")]
        [StringLength(1200, ErrorMessage = "Description Must Be Shorter Than 400 Characters")]
        public string Description { get; set; }

        public int OpeningCount { get; set; }

        [Required(ErrorMessage = "Position Type is Required")]
        public string PositionType { get; set; }
        public int PositionID { get; set; }
        

        public virtual ICollection<Application> Applications { get; set; }

        public virtual Position Position { get; set; }

       

       

        public virtual ICollection<School> Schools { get; set; }

        
        public virtual ICollection<Skill> Skills { get; set; }

       

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ClosingDate < DateTime.Today)
            {
                yield return new ValidationResult("The closing date cannot be in the past.", new[] { "ClosingDate" });
            }
            if (StartDate.GetValueOrDefault() < System.DateTime.Today)
            {
                yield return new ValidationResult("The start date for the posting cannot be before the current date.", new[] { "StartDate" });
            }
        }



       
        public virtual ICollection<Qualification> Qualifications { get; set; }

    }
}
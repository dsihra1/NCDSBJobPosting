using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

//testing

namespace CommunityProject_ProjectUnity.Models
{
   
    public class Applicant : Auditable
    {
        public Applicant()
        {
            this.Applications = new HashSet<Application>();
            this.Skills = new HashSet<Skill>();
            this.Files = new HashSet<aFile>();
            this.Qualifications = new HashSet<Qualification>();
        }

        [Display(Name = "Applicant")]
        public string Summary
        {
            get
            {
                return FormalName + " - email: " + Email;
            }
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name Must Be Shorter Than 50 Characters")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Middle name cannot be more than 30 characters long.")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name Must Be Shorter Than 50 Characters")]
        public string LastName { get; set; }

        [Display(Name = "Applicant Name")]
        public string FormalName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Display(Name = "Applicant")]
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
        [Index("IX_Uniqe_Email", IsUnique = true)]
        [Required(ErrorMessage = "Email Address Required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150, ErrorMessage = "Last Name Must Be Shorter Than 150 Characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}")]
        [RegularExpression("^\\d{10}$")]
        public string Phone { get; set; }


        //Added to hold picture of applicant
        public virtual ApplicantImage ApplicantImage { get; set; }

        //Added to hold related files
        public ICollection<aFile> Files { get; set; }
        public int QualificationID { get; set; }

        
      
        public ICollection<Application> Applications { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

        public virtual ICollection<Qualification> Qualifications { get; set; }
    }
}
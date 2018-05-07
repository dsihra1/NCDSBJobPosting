using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CommunityProject_ProjectUnity.Models
{
   [Table("School")]
    public class School
    {

        public School()
        {
            //this.Postings = new HashSet<Posting>();
            this.Positions = new HashSet<Position>();
                
        }

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = " Name is Required")]
        [Display(Name = "School")]
        [StringLength(150, ErrorMessage = "Name Must Be Shorter Than 150 Characters")]
        public string Name { get; set; }

        [Display(Name = "ShortName")]
        public string Shortname
        {
            get
            {

                return Name.Remove(Name.Length - 6);
                
            }
        }

        
        

        public virtual ICollection<Posting> Postings { get; set; }

        public virtual ICollection<Position> Positions { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class Log
    {
        [Key]
        [Display(Name = "Log ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Activity")]
        public string Operation { get; set; }
        [Required]   
        public string Category { get; set; }
        [Required]
        public string StaffID { get; set; }
        [Display(Name = "Activity Date")]
        public DateTime ActionTime { get; set; }
        public Staff Staff { get; set; }
    }
}

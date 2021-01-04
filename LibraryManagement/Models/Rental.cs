using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class Rental
    {
        [Key]
        [Display(Name = "Rental ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Book ID")]
        public string BookID { get; set; }
        [Required]
        [Display(Name = "Member ID")]
        public string MemberID { get; set; }
        [Required]
        [Display(Name = "Staff ID")]
        public string StaffID { get; set; }
        [Required]
        [Display(Name = "Rent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RentDate { get; set; }
        [Required]
        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReturnDate { get; set; }
        public bool Status { get; set; } = false;
        public Collection Collection { get; set; }
        public Member Member { get; set; }
        public Staff Staff { get; set; }
    }
}

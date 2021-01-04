using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class Collection
    {
        [Key]
        [Display(Name = "Book ID")]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Published Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}",ApplyFormatInEditMode =true)]
        public DateTime PublishedDate { get; set; }
        [Display(Name = "Book Category")]
        public string Category { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}

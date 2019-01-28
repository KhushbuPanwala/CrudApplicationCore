using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFMVCCoreSampleApp.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public int AuthorId { get; set; }
        [Display(Name ="Author Name")]
        public Author Author { get; set; }
    }
}

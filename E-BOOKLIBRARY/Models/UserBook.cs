using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public class UserBook
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public AppUser User { get; set; }
        [Required]
        public string BookId { get; set; }
        [Required]
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}

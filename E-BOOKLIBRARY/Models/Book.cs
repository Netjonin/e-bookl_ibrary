using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public class Book
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "This is a required field.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This is a required field.")]
        public string ISBN { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "This is a required field.")]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string Image { get; set; }

        public List<UserBook> UserBooks { get; set; }

        public Book()
        {
            UserBooks = new List<UserBook>();
        }
    }
}

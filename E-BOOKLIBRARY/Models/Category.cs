using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "This is a required field.")]
        public string Name { get; set; }

        public List<Book> Books { get; set; }

        public Category()
        {
            Books = new List<Book>();
        }
    }
}

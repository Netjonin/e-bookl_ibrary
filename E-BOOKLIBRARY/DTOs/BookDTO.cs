using E_BOOKLIBRARY.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.DTOs
{
    public class BookDTO
    {
        [Required(ErrorMessage = "This is a required field.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This is a required field.")]
        public string ISBN { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.DTOs
{
    public class CatDTO
    {
         [Required(ErrorMessage = "Name is a required field")]
         public string Name { get; set; }
    }
}

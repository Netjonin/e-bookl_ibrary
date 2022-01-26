using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.DTOs
{
    public class RegisterDto
    {


        [Display(Name = "First name")]
        [Required(ErrorMessage = "Full name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Full name is required")]
        public string LastName { get; set; }


        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }



        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Comfirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ComfirmPassword { get; set; }
    }
}

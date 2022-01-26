using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Models
{
    public class AppUser : IdentityUser
    {

        [Required(ErrorMessage = "This is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length is 60 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length is 60 characters.")]
        public string LastName { get; set; }

        
        public string Password { get; set; }
        
        
        public string ComfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<UserBook> UserBooks { get; set; }
        public AppUser()
        {
            UserBooks = new List<UserBook>();
        }


    }
}

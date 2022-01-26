using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{

    public interface IAuthService
    {
        Task<LoginCredDTO> Login(string email, string password, bool rememberMe);
        Task<string> GenerateEmailConfirmationToken(AppUser user);
        Task<IdentityResult> ConfirmEmail(AppUser user, string token);
        Task<IdentityResult> Register(AppUser user, string password);
    }
}

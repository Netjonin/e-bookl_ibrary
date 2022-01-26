using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signinMgr;
        private readonly IJWTService _jwtServices;
        private readonly UserManager<AppUser> _userMgr;

        public AuthService(IJWTService jwtService, 
            SignInManager<AppUser> signinMgr,
            UserManager<AppUser> userManager)
        {
            _jwtServices = jwtService;
            _signinMgr = signinMgr;
            _userMgr = userManager;
        }

        public async Task<LoginCredDTO> Login(string email, string password, bool rememberMe)
        {
            var user = await _userMgr.FindByEmailAsync(email);
            var res = await _signinMgr.PasswordSignInAsync(user, password, rememberMe, false);

            if (!res.Succeeded)
            {
                return new LoginCredDTO { Status = false };
            }

            var userRoles = await _userMgr.GetRolesAsync(user);
            var claims = await _userMgr.GetClaimsAsync(user);
            var token = _jwtServices.GenerateToken(user, userRoles.ToList(), claims);

            return new LoginCredDTO { Status = true, Id = user.Id, Token = token };
        }

        public async Task<string> GenerateEmailConfirmationToken(AppUser user)
        {
            return await _userMgr.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmail(AppUser user, string token)
        {
            var res = await _userMgr.ConfirmEmailAsync(user, token);
            if (res.Succeeded)
            {
                user.IsActive = true;
            }
            return res;
        }

        public async Task<IdentityResult> Register(AppUser user, string password)
        {
            return await _userMgr.CreateAsync(user, password);
        }
    }
}

using AutoMapper;
using E_BOOKLIBRARY.Commons;
using E_BOOKLIBRARY.Data.Db;
using E_BOOKLIBRARY.Data.Static;
using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Models;
using E_BOOKLIBRARY.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly UserManager<AppUser> _userManager;
        //private readonly BookLibraryContext _context;
        //private readonly SignInManager<AppUser> _signInManager;

        //public AuthController(UserManager<AppUser> userManager, BookLibraryContext context, SignInManager<AppUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _context = context;
        //    _signInManager = signInManager;
        //}

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDto model)
        //{
        //    if (!ModelState.IsValid) return Ok("Invalid details");

        //    var user = await _userManager.FindByEmailAsync(model.EmailAddress);

        //    if (user != null)
        //    {
        //        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);

        //        if (passwordCheck)
        //        {
        //            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        //            if (result.Succeeded)
        //            {
        //                return Ok(user);
        //            }
        //        }
        //        return Ok("Wrong credentials");
        //    }

        //    return Ok("Wrong credentials");

        //}

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(registerDto model)
        //{
        //    if (!ModelState.IsValid) return Ok("Invalid details");

        //    var user = await _userManager.FindByEmailAsync(model.EmailAddress);

        //    if (user != null)
        //        return Ok("Email already exist");

        //    var newUser = new AppUser()
        //    {
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        Email = model.EmailAddress,
        //        UserName = model.EmailAddress
        //    };


        //    var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);

        //    if (newUserResponse.Succeeded)
        //    {

        //        await _userManager.AddToRoleAsync(newUser, UserRoles.User);

        //        return Ok("user successfully registered");
        //    }
        //    else
        //    {
        //        return Ok("user successfully registered");
        //    }
        //}

        //[HttpPost("Logout")]
        //public async Task<IActionResult> LogOut()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok("Successfully logged out");
        //}

        private readonly UserManager<AppUser> _userMgr;
        private readonly IAuthService _authService;
        public AuthController(IAuthService auth, UserManager<AppUser> userManager)
        {
            _userMgr = userManager;
            _authService = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {

            var user = await _userMgr.FindByEmailAsync(model.EmailAddress);
            if (user == null)
            {
                ModelState.AddModelError("Invalid", "Credentials provided by the user is invalid");
                return BadRequest(Util.BuildResponse<object>(false, "Invalid credentials", ModelState, null));
            }

            // check if user's email is confirmed
            if (await _userMgr.IsEmailConfirmedAsync(user))
            {
                var res = await _authService.Login(model.EmailAddress, model.Password, model.RememberMe);

                if (!res.Status)
                {
                    ModelState.AddModelError("Invalid", "Credentials provided by the user is invalid");
                    return BadRequest(Util.BuildResponse<object>(false, "Invalid credentials", ModelState, null));
                }

                return Ok(Util.BuildResponse(true, "Login is sucessful!", null, res));
            }

            ModelState.AddModelError("Invalid", "User must first confirm email before attempting to login");
            return BadRequest(Util.BuildResponse<object>(false, "Email not confirmed", ModelState, null));
        }
    }

}

using AutoMapper;
using E_BOOKLIBRARY.Commons;
using E_BOOKLIBRARY.Data.Static;
using E_BOOKLIBRARY.DTOs;
using E_BOOKLIBRARY.Helpers;
using E_BOOKLIBRARY.Models;
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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userMgr;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _userMgr = userManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(RegisterDto model)
        {
            // if user already exist return early
            var existingEmailUser = await _userMgr.FindByEmailAsync(model.Email);
            if (existingEmailUser != null)
            {
                ModelState.AddModelError("Invalid", $"User with email: {model.Email} already exists");
                return BadRequest(Util.BuildResponse<object>(false, "User already exists!", ModelState, null));
            }

            // map data from model to user
            // var user = _mapper.Map<AppUser>(model);
            var user = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = model.Password
            };
            
            //user.Address.Street = model.Street;
            //user.Address.State = model.State;
            //user.Address.Country = model.Country;


            var response = await _userMgr.CreateAsync(user, model.Password);

            if (!response.Succeeded)
            {
                foreach (var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user!", ModelState, null));
            }

            var res = await _userMgr.AddToRoleAsync(user, UserRoles.User);

            if (!res.Succeeded)
            {
                foreach (var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user role!", ModelState, null));
            }

            // if you system requires user's email to be confirmed before they can login 
            // you can generate confirm email token here
            // but ensure AddDefaultTokenProviders() have been enabled in startup else there won't be token generated
            var token = await _userMgr.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("ConfrimEmail", "User", new { Email = user.Email, Token = token }, Request.Scheme);  // this is the url to send

            // next thing TODO here is to send an email to this new user to the email provided using a notification service you should build

            // map data to dto
            var details = _mapper.Map<RegisterSuccessDto>(user);

            // the confirmation link is added to this response object for testing purpose since at this point it is not being sent via mail
            return Ok(Util.BuildResponse(true, "New user added!", null, new { details, ConfimationLink = url }));
        }
        [HttpGet("get-users")]
        public IActionResult GetUsers(int page, int perPage)
        {
            var listOfUsersToReturn = new List<UserToReturnDto>();
            var users = _userMgr.Users.ToList();
            if (users != null)
            {
                var pagedList = PagedList<AppUser>.Paginate(users, page, perPage);
                foreach (var user in pagedList.Data)
                {
                    listOfUsersToReturn.Add(_mapper.Map<UserToReturnDto>(user));
                }
                var res = new PaginatedListDto<UserToReturnDto>
                {
                    MetaData = pagedList.MetaData,
                    Data = listOfUsersToReturn
                };

                return Ok(Util.BuildResponse(true, "List of users", null, res));


            }
            else
            {
                ModelState.AddModelError("Not found", "There was not a record of user found!");
                var res = Util.BuildResponse<List<UserToReturnDto>>(false, "No result found!", ModelState, null);
                return NotFound(res);
            }
        }

        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetUser(string email)
        {
            // map data from db to dto to reshape it and remove null fields
            var UserToReturn = new UserToReturnDto();
            //var user = await _userService.GetUser(email);
            var user = await _userMgr.FindByEmailAsync(email);
            if (user != null)
            {
                UserToReturn = new UserToReturnDto
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email
                };

                var res = Util.BuildResponse(true, "User details", null, UserToReturn);
                return Ok(res);
            }
            else
            {
                ModelState.AddModelError("Notfound", $"There was no record found for user with email {user.Email}");
                var res = Util.BuildResponse<List<UserToReturnDto>>(false, "No result found!", ModelState, null);
                return NotFound(res);
            }

        }
        
    }
}

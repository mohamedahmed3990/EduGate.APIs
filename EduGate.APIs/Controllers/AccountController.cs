using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using EduGate.Core.Entities.Identity;
using EduGate.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduGate.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           _authService = authService;
        } 


        [HttpPost("login")]  // post: /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if(result.Succeeded is false) 
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpPost("register")]  // post: /api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() {Errors = new string[] { "This Email is already in exist!!" } });
            
            if (CheckUserName(model.Email.Split('@')[0]).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() {Errors = new string[] { "This UserName is already in exist!!" } });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
                PictureUrl = $"Upload/student/{model.Email.Split('@')[0]}.png"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            });
        }


        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        private async Task<ActionResult<bool>> CheckUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) is not null;
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            user.DisplayName = updateUserDto.DisplayName;
            user.PhoneNumber = updateUserDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);


            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            });
        }



        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully :(" });
        }

    }
}

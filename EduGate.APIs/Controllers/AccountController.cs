using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using EduGate.Core;
using EduGate.Core.Entities;
using EduGate.Core.Entities.Identity;
using EduGate.Core.Repositories.Contract;
using EduGate.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using System.Security.Claims;

namespace EduGate.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IAuthService authService,RoleManager<IdentityRole> roleManager, IDoctorRepository doctorRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           _authService = authService;
            _roleManager = roleManager;
            _doctorRepository = doctorRepository;
            _unitOfWork = unitOfWork;
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


            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Doctor"))
            {
                // Retrieve the doctor's ID from the database
                var doctor = await _doctorRepository.GetbyUserId(user.Id);
                if (doctor is null || doctor.IsActive == false)
                    return Unauthorized(new ApiResponse(400));

                var token = await _authService.CreateTokenAsync(user, _userManager, doctor.Id);


                return Ok(new UserDto()
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = token
                });
            }
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
        public async Task<ActionResult<UserDto>> StudentRegister(RegisterDto model)
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

            var roleExists = await _roleManager.RoleExistsAsync("Student");
            if (!roleExists)
            {
                return BadRequest(new ApiResponse(400, "Student role does not exist"));
            }

            await _userManager.AddToRoleAsync(user, "Student");

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }



        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser(CreateUserModel userModel)
        {
            if (CheckEmailExists(userModel.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email is already in exist!!" } });

            var roleExists = await _roleManager.RoleExistsAsync(userModel.Role);
            if (!roleExists)
            {
                return BadRequest(new ApiResponse(400, "Role dose not exist"));
            }

            var user = new AppUser()
            {
                DisplayName = userModel.DisplayName,
                UserName = userModel.Email.Split('@')[0],
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded is false)
                return BadRequest(new ApiResponse(400));

            await _userManager.AddToRoleAsync(user, userModel.Role);

            if (userModel.Role.Contains("Doctor"))
            {
                var doctor = new Doctor()
                {
                    Name = userModel.DisplayName,
                    UserName = user.Email.Split("@")[0],
                    UserId = user.Id,
                    IsActive = true
                };
                await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
                await _unitOfWork.CompleteAsync();
            }

            return Ok(new UserAdminToReturnDto
            {
                DisplayName = userModel.DisplayName,
                Email = userModel.Email,
                UserName = userModel.Email.Split("@")[0],
                PhoneNumber = userModel.PhoneNumber,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }




        [Authorize(Roles = "Admin")]
        [HttpGet("GetStudentUsers")]
        public async Task<ActionResult<StudentAccountToReturnDto>> GetStudents()
        {
            var users = await _userManager.GetUsersInRoleAsync("Student");

            var result = users.Select(user => new StudentAccountToReturnDto
            {
                Name = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
            });
            
            return Ok(result);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetDoctorAndAdminUsers")]
        public async Task<ActionResult> GetUsers()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var doctorUser = doctors.Select(user => new DoctorAccountToReturnDto
            {
                Name = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = "Doctor"
            });

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminUser = admins.Select(user => new DoctorAccountToReturnDto
            {
                Name = user.DisplayName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = "Admin"
            });

            var result = doctorUser.Concat(adminUser);
            return Ok(result);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("AllRoles")]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var result = roles.Select(role => role.Name).ToList();

            return Ok(result);
        }


        #region Testing

        //[HttpGet("GetAllUsers")]
        //public async Task<ActionResult> GetAllUsers()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    return Ok(users);
        //}


        //[HttpPost("AddRole")]
        //public async Task<ActionResult> CreateRole(string name)
        //{
        //    var roleExist = await _roleManager.RoleExistsAsync(name);
        //    if (roleExist) return BadRequest(new ApiResponse(400, "Role already exit"));

        //    var rolseResult = await _roleManager.CreateAsync(new IdentityRole(name));
        //    return Ok(new ApiResponse(200, "Role added successfully"));

        //}

        //[HttpGet("GetAdminUsers")]
        //public async Task<ActionResult> GetAdmins()
        //{
        //    var users = await _userManager.GetUsersInRoleAsync("Admin");
        //    return Ok(users);
        //}


        //[HttpPost("AddUsersToRole")]
        //public async Task<ActionResult> AddUserToRole(string email, string roleName)
        //{
        //    // check if user is exist 
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user is null) return BadRequest(new ApiResponse(400, "User dose not exist"));


        //    // check if role is exist 
        //    var role = await _roleManager.RoleExistsAsync(roleName);
        //    if (!role) return BadRequest(new ApiResponse(400, "Role dose not exist"));

        //    // check if user is assign to role
        //    var result = await _userManager.AddToRoleAsync(user, roleName);

        //    return Ok(new ApiResponse(200, $"Successfully add user: {user} to role: {roleName}"));


        //}


        //[HttpGet("GetUserRole")]
        //public async Task<ActionResult> GetUserRole(string email)
        //{
        //    // check email if exist
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user is null) return BadRequest(new ApiResponse(400, "user dose not exist"));

        //    var role = await _userManager.GetRolesAsync(user);

        //    return Ok(role);
        //}


        #endregion



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

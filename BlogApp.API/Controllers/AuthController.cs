using AutoMapper;
using BlogApp.API.Data.Entities;
using BlogApp.API.DTOs.Request;
using BlogApp.API.DTOs.Response;
using BlogApp.API.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly TokenProvider _tokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthController(
            TokenProvider tokenProvider,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper
            )
        {
            _signInManager = signInManager;
            _tokenProvider = tokenProvider;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpPost("register")]
        public async Task <IActionResult> Register(RegisterDto dto)
        {
           var userExist = await _userManager.FindByEmailAsync(dto.Email);
           if(userExist != null)
           {
                return BadRequest(Response<object>.Fail("User with this Email Already Exists"));
           }

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            var authResponse = new AuthResponseDto
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = "User"
            };

            return Ok(Response<AuthResponseDto>.Ok(authResponse, "User Registered Successfully"));

        }

        [HttpPost("login")]
        public async Task <IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest(Response<object>.Fail("User with this Email does not Exists"));
            }

            var result = await _signInManager.PasswordSignInAsync(
                user: user,
                password: dto.Password,
                isPersistent: true,
                lockoutOnFailure: false
            );

            var role = await _userManager.GetRolesAsync(user);

            var token = await _tokenProvider.CreateToken(user.Id, user.FullName, user.Email, role[0]);

            var authResponse = new AuthResponseDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = role[0],
                Token = token
            };

            return Ok(Response<AuthResponseDto>.Ok(authResponse, "User logged In..."));
        }
    }
}

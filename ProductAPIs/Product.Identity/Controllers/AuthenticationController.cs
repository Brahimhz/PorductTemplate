using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product.Core.Models;
using Product.Identity.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCorsPolicy")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        private const string DefaultRole = "User";
        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            //Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
                return BadRequest("User already exists!");

            User user = new();

            try
            {
                user = _mapper.Map<RegisterUser, User>(registerUser);
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }

            if (await _roleManager.RoleExistsAsync(DefaultRole))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Content = "User Failed to Create",
                        ContentType = "text/plain"
                    };

                await _userManager.AddToRoleAsync(user, DefaultRole);
                return Ok();
            }
            else
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = "This Role Doesnot Exist.",
                    ContentType = "text/plain"
                };
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request");

            var user = await _userManager.FindByNameAsync(model.Username)
                           ?? await _userManager.FindByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                UserDto userDto = new();
                try
                {
                    userDto = _mapper.Map<User, UserDto>(user);
                }
                catch (Exception ex)
                {
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Content = ex.Message,
                        ContentType = "text/plain"
                    };
                }

                if (result.Succeeded)
                {
                    userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();

                    var token = CreateToken(userDto);

                    return Ok(new
                    {
                        Token = token,
                        Mesage = "Login Success!"
                    });
                }
            }

            // Authentication failed
            return Unauthorized("Invalid username or password");
        }


        private string CreateToken(UserDto user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var sercretKey = _configuration.GetSection("SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(sercretKey);
            var identity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, string.Concat(user.Roles)),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Name, $"{user.UserName}")
                });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }




        [HttpGet("Public")]
        public IActionResult Public() => Ok("Public");

        [HttpGet("Private")]
        [Authorize]
        public IActionResult Private() => Ok("Private");


        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => Ok("Admin");

    }
}

using AutoMapper;
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
        private readonly ILogger<AuthenticationController> _logger;
        private const string DefaultRole = "User";
        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation RegisterUser Model input Error.");
                return BadRequest("Invalid Register request");
            }

            //Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                _logger.LogError("Validation Email register Error.");
                return BadRequest("User already exists!");
            }

            User user = new();

            try
            {
                user = _mapper.Map<RegisterUser, User>(registerUser);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During the mapping :" + ex.Message);
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }

            try
            {
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
            catch (Exception e)
            {
                // Register failed
                _logger.LogError("Register proccess Error :" + e.Message);
                return BadRequest("Register proccess Error ");
            }


        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation LoginModel input Error.");
                return BadRequest("Invalid login request");
            }

            try
            {
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
                        _logger.LogError("Error During the mapping :" + ex.Message);
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

                        _logger.LogInformation("Login Success.");

                        return Ok(new
                        {
                            Token = token,
                            Mesage = "Login Success!"
                        });
                    }
                    else
                    {

                        // Authentication failed
                        _logger.LogError("Validation password signin Error.");
                        return Unauthorized("Invalid password");
                    }
                }
                else
                {

                    // Authentication failed
                    _logger.LogError("Validation username signin Error");
                    return Unauthorized("Invalid username");
                }

            }
            catch (Exception e)
            {
                // Authentication failed
                _logger.LogError("Login proccess Error :" + e.Message);
                return BadRequest("Login proccess Error");
            }


        }


        private string CreateToken(UserDto user)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var sercretKey = _configuration.GetSection("SecretKey").Value;
                var key = Encoding.ASCII.GetBytes(sercretKey);
                var identity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Role, string.Concat(user.Roles)),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName + "."+ user.LastName)
                    });
                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);

                _logger.LogInformation("Create Token with Success");
                return jwtTokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                // Authentication failed
                _logger.LogError("Create Token proccess Error :" + e.Message);
                return null;
            }

        }


    }
}

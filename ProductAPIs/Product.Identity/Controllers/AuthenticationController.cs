using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product.Core.Models;
using Product.Identity.DTOs;

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
        private const string DefaultRole = "User";
        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            SignInManager<User> signInManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
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
                    return Ok(userDto);
            }

            // Authentication failed
            return Unauthorized("Invalid username or password");

        }
    }
}

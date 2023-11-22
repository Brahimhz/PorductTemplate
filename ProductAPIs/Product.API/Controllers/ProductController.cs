using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

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

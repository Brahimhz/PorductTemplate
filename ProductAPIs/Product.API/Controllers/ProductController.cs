using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {

        private readonly IProductAppService _appService;

        public ProductController(
            IProductAppService appService)
        {
            _appService = appService;
        }

        [HttpPost(Name = "CreateProduct")]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Product data");

            var result = await _appService.Add(input);
            if (result is null) return BadRequest();
            else return Ok(result);
        }

        [HttpPut(Name = "ModifyProduct/{id}")]
        [Authorize]
        public async Task<IActionResult> ModifyProduct(Guid id, [FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Product data");

            var result = await _appService.Modify(id, input);
            if (result is null) return BadRequest();
            else return Ok(result);
        }

        [HttpDelete(Name = "DeleteProduct/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _appService.Delete(id);
            return Ok(result);
        }

        [HttpGet("Product/{id}", Name = "GetProductById")]
        [Authorize]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _appService.GetById(id);
            if (result is null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("Products", Name = "GetProducts")]
        [Authorize]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _appService.GetAll();
            if (result is null) return BadRequest();
            return Ok(result);
        }



    }
}

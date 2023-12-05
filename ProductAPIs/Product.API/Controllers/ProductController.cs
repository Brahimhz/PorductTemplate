using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductAppService appService,
            ILogger<ProductController> logger)
        {
            _appService = appService;
            _logger = logger;
        }

        [HttpPost(Name = "CreateProduct")]
        [Authorize(Policy = "StoreOwner")]
        [Authorize(Policy = "StoreOwnerRole")]
        [EnableCors("MyCorsPolicy")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product data");
            }

            var result = await _appService.Add(input);
            if (result is null) return BadRequest();
            else return Ok(result);
        }

        [HttpPut(Name = "ModifyProduct/{id}")]
        [Authorize]
        public async Task<IActionResult> ModifyProduct(Guid id, [FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product data");
            }

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


        [HttpGet("Categories", Name = "GetCategories")]
        [Authorize]
        public async Task<IActionResult> GetCategories()
        {
            var categories = new List<string> { "Category 1", "Category 2", "Category 3", "Category 4", "Category 5", "Category 6" };
            return Ok(categories);
        }



    }
}

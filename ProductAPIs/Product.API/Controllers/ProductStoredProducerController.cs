using Microsoft.AspNetCore.Mvc;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;
using Product.Core.Models;
using System.ComponentModel.DataAnnotations;
using ProductEntity = Product.Core.Models;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoredProducerController : ControllerBase
    {
        private readonly ISpProductAppService _appService;
        private readonly IGenericSpAppService<ProductEntity.Product, ProductInsert, ProductOutPut, ProductOutPutList, ProductInPut> _productAppService;
        private readonly ILogger<ProductStoredProducerController> _logger;

        public ProductStoredProducerController(
            ISpProductAppService appService,
            IGenericSpAppService<ProductEntity.Product, ProductInsert, ProductOutPut, ProductOutPutList, ProductInPut> productAppService,
            ILogger<ProductStoredProducerController> logger)
        {
            _appService = appService;
            _productAppService = productAppService;
            _logger = logger;
        }

        [HttpGet("GetAllRecords")]
        public async Task<IActionResult> GetAllRecords()
            => Ok(await _appService.GetAll());

        [HttpGet("GetById/{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            var result = await _appService.GetById(productId);

            return result is null ? NotFound() : Ok(result);
        }


        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProductById([FromQuery][Required] Guid id, [FromBody][Required] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product Input");
            }


            var result = await _appService.Modify(id, input);

            return result == 0 ? NotFound() : Ok("Product updated successfully");
        }


        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product Input");
            }


            var result = await _appService.Add(input);

            return result == 0 ? BadRequest() : Ok("Product inserted successfully");
        }

        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById([FromQuery][Required] Guid productId)
        {
            var result = await _appService.Delete(productId);

            return result == 0 ? NotFound() : Ok("Product deleted successfully");

        }



        //************** Generic *********************


        [HttpGet("GetAllGeneric")]
        public async Task<IActionResult> GetAllGeneric()
            => Ok(await _productAppService.GetAll());

        [HttpGet("GetByIdGeneric/{productId}")]
        public async Task<IActionResult> GetByIdGeneric(Guid productId)
        {
            var result = await _productAppService.GetById(productId);

            return result is null ? NotFound() : Ok(result);
        }


        [HttpPut("UpdateProductGeneric")]
        public async Task<IActionResult> UpdateProductByIdGeneric([FromQuery][Required] Guid id, [FromBody][Required] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product Input");
            }


            var result = await _productAppService.Modify(id, input);

            return result == 0 ? NotFound() : Ok("Product updated successfully");
        }


        [HttpPost("InsertProductGeneric")]
        public async Task<IActionResult> InsertProductGeneric([FromBody] ProductInPut input)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Validation Product Model input Error.");
                return BadRequest("Invalid Product Input");
            }


            var result = await _productAppService.Add(input);

            return result == 0 ? BadRequest() : Ok("Product inserted successfully");
        }

        [HttpDelete("DeleteProductByIdGeneric")]
        public async Task<IActionResult> DeleteProductByIdGeneric([FromQuery][Required] Guid productId)
        {
            var result = await _productAppService.Delete(productId);

            return result == 0 ? NotFound() : Ok("Product deleted successfully");

        }

    }
}

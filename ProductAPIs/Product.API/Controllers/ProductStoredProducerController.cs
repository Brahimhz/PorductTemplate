using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Product.API.AppService.Dtos.Product;
using Product.Persistence;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoredProducerController : ControllerBase
    {
        private readonly ProductDbContext _context;

        private const string StoredProducer_GetAllRecords = "GetAllRecords";
        private const string StoredProducer_GetById = "GetById ";

        public ProductStoredProducerController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllRecords")]
        public async Task<IActionResult> GetAllRecords()
        {
            var result = await _context.Products.FromSqlRaw(StoredProducer_GetAllRecords).ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetById/{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            var result = await _context.Products.FromSqlRaw("EXEC " + StoredProducer_GetById + " @ProductId", new SqlParameter("@ProductId", productId)).ToListAsync();

            if (result is null) return NotFound();
            if (!result.Any()) return NotFound();

            return Ok(result);
        }


        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProductById([FromQuery][Required] Guid id, [FromBody][Required] ProductInPut input)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateProductById @Id, @NewName, @NewCategory, @NewAmount, @NewIsActive",
                new SqlParameter("@Id", id),
                new SqlParameter("@NewName", input.Name),
                new SqlParameter("@NewCategory", input.Category),
                new SqlParameter("@NewAmount", input.Amount),
                new SqlParameter("@NewIsActive", input.IsActive)
            );

            if (result == 0)
            {
                return NotFound();
            }

            return Ok("Product updated successfully");
        }


        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInPut input)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC InsertProduct @Name, @Category, @Amount, @IsActive",
                new SqlParameter("@Name", input.Name),
                new SqlParameter("@Category", input.Category),
                new SqlParameter("@Amount", input.Amount),
                new SqlParameter("@IsActive", input.IsActive)
            );

            if (result == 0)
            {
                return BadRequest();
            }

            return Ok("Product inserted successfully");
        }

        [HttpDelete("DeleteProductById")]
        public async Task<IActionResult> DeleteProductById([FromQuery][Required] Guid productId)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC DeleteProductById @ProductId",
                new SqlParameter("@ProductId", productId)
            );

            if (result == 0)
            {
                return NotFound(); // Return a 404 if the product is not found
            }

            return Ok("Product deleted successfully");
        }

    }
}

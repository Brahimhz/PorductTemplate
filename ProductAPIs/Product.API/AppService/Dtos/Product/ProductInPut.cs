using System.ComponentModel.DataAnnotations;

namespace Product.API.AppService.Dtos.Product
{
    public class ProductInPut
    {
        [MaxLength(100)]
        [MinLength(0)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public Guid OwnerId { get; set; }
    }
}

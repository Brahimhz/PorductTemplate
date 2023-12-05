using System.ComponentModel.DataAnnotations;

namespace Product.Core.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool isActive { get; set; }
        public Guid OwnerId { get; set; }
    }

    public class ProductInsert
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public bool isActive { get; set; }
    }
}

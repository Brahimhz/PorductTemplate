using System.ComponentModel.DataAnnotations;

namespace Product.Core.Models
{
    public interface BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Product.Core.Models
{
    public class User : IdentityUser<Guid>, BaseEntity
    {
        [Key]
        public override Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.Core.Models;

namespace Product.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {

        }

    }


    public class ProductIdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ProductIdentityDbContext(DbContextOptions<ProductIdentityDbContext> options)
            : base(options)
        {

        }

    }
}

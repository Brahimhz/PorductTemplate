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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>().HasData
                (
                    new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
        }

    }
}

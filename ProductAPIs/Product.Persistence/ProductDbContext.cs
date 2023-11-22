using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.Core.Models;
using ProductEntity = Product.Core.Models;

namespace Product.Persistence
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {

        }


        public DbSet<ProductEntity.Product> Products { get; set; }

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
            var adminRoleId = SeedRoles(builder);
            SeedAdminUser(builder, adminRoleId);
        }

        private void SeedAdminUser(ModelBuilder builder, Guid adminRoleId)
        {
            var adminUserId = Guid.NewGuid();
            builder.Entity<User>().HasData(
                            new User
                            {
                                Id = adminUserId,
                                FirstName = "Admin",
                                LastName = "Admin",
                                UserName = "admin@example.com",
                                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                                Email = "admin@example.com",
                                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                                EmailConfirmed = true,
                                PasswordHash = new PasswordHasher<IdentityUser<Guid>>().HashPassword(null, "Password123+"),
                                SecurityStamp = string.Empty,
                                LastUpdateDate = DateTime.Now,
                                CreationDate = DateTime.Now
                            }
                        );

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );

        }

        private Guid SeedRoles(ModelBuilder builder)
        {
            var adminRoleId = Guid.NewGuid();
            builder.Entity<IdentityRole<Guid>>().HasData
                (
                    new IdentityRole<Guid>() { Id = adminRoleId, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );

            return adminRoleId;
        }

    }
}

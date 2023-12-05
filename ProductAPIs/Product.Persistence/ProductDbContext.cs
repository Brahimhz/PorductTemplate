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


        public DbSet<ProductEntity.Product> Product { get; set; }

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
            //var adminRoleId = SeedRoles(builder);
            //SeedAdminUser(builder, adminRoleId);

            //var storeOwnerRoleId = SeedStoreOwnerRoles(builder);
            //SeedStoreOwners(builder, storeOwnerRoleId);

            //var simpleUserRoleId = SeedSimpleUserRoles(builder);
            //SeedSimpleUsers(builder, simpleUserRoleId);

        }

        private Guid SeedSimpleUserRoles(ModelBuilder builder)
        {
            var simpleUserRoleId = Guid.NewGuid();
            builder.Entity<IdentityRole<Guid>>().HasData
                (
                    new IdentityRole<Guid>() { Id = simpleUserRoleId, Name = "SimpleUser", ConcurrencyStamp = "1", NormalizedName = "SimpleUser" }
                );

            return simpleUserRoleId;
        }
        private void SeedSimpleUsers(ModelBuilder builder, Guid simpleUserRoleId)
        {
            var SimpleUsers1UserId = Guid.NewGuid();
            builder.Entity<User>().HasData(
                            new User
                            {
                                Id = SimpleUsers1UserId,
                                FirstName = "SimpleUsers1",
                                LastName = "SimpleUsers1",
                                UserName = "SimpleUsers1@example.com",
                                NormalizedUserName = "SimpleUsers1@EXAMPLE.COM",
                                Email = "SimpleUsers1@example.com",
                                NormalizedEmail = "SimpleUsers1@EXAMPLE.COM",
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
                    RoleId = simpleUserRoleId,
                    UserId = SimpleUsers1UserId
                }
            );

            var SimpleUsers2UserId = Guid.NewGuid();
            builder.Entity<User>().HasData(
                            new User
                            {
                                Id = SimpleUsers2UserId,
                                FirstName = "SimpleUsers2",
                                LastName = "SimpleUsers2",
                                UserName = "SimpleUsers2@example.com",
                                NormalizedUserName = "SimpleUsers2@EXAMPLE.COM",
                                Email = "SimpleUsers2@example.com",
                                NormalizedEmail = "SimpleUsers2@EXAMPLE.COM",
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
                    RoleId = simpleUserRoleId,
                    UserId = SimpleUsers2UserId
                }
            );

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

        private void SeedStoreOwners(ModelBuilder builder, Guid storeOwnerRoleId)
        {
            var StoreOwner1UserId = Guid.NewGuid();
            builder.Entity<User>().HasData(
                            new User
                            {
                                Id = StoreOwner1UserId,
                                FirstName = "StoreOwner1",
                                LastName = "StoreOwner1",
                                UserName = "StoreOwner1@example.com",
                                NormalizedUserName = "StoreOwner1@EXAMPLE.COM",
                                Email = "StoreOwner1@example.com",
                                NormalizedEmail = "StoreOwner1@EXAMPLE.COM",
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
                    RoleId = storeOwnerRoleId,
                    UserId = StoreOwner1UserId
                }
            );

            var StoreOwner2UserId = Guid.NewGuid();
            builder.Entity<User>().HasData(
                            new User
                            {
                                Id = StoreOwner2UserId,
                                FirstName = "StoreOwner2",
                                LastName = "StoreOwner2",
                                UserName = "StoreOwner2@example.com",
                                NormalizedUserName = "StoreOwner2@EXAMPLE.COM",
                                Email = "StoreOwner2@example.com",
                                NormalizedEmail = "StoreOwner2@EXAMPLE.COM",
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
                    RoleId = storeOwnerRoleId,
                    UserId = StoreOwner2UserId
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

        private Guid SeedStoreOwnerRoles(ModelBuilder builder)
        {
            var storeOwnerRoleId = Guid.NewGuid();
            builder.Entity<IdentityRole<Guid>>().HasData
                (
                    new IdentityRole<Guid>() { Id = storeOwnerRoleId, Name = "StoreOwner", ConcurrencyStamp = "1", NormalizedName = "STOREOWNER" }
                );

            return storeOwnerRoleId;
        }

    }
}

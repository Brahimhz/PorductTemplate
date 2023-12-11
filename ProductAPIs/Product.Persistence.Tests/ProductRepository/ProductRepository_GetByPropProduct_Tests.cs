using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Product.Core.Data.IRepository;
using Product.Persistence.Migrations.ProductDb;
using Product.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = Product.Core.Models.Product;

namespace Product.Persistence.Tests.ProductRepository
{
    public class ProductRepository_GetByPropProduct_Tests
    {
        [Fact]
        public async Task GetByIdProduct_ReturnNotNullProduct()
        {
            var optionbuilder = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var context = new ProductDbContext(optionbuilder.Options);

            var repository = new GenericRepository<ProductEntity>(context);

            var id = Guid.NewGuid();

            await repository.InsertAsync(new ProductEntity
            {
                Id = id,
                Amount = 1,
                Category = "Category",
                isActive = true,
                Name = "Test",
                OwnerId = Guid.NewGuid()
            });

            var product = await repository.GetByPropAsync(p => p.Id == id);

            Assert.NotNull(product);
        }
    }
}

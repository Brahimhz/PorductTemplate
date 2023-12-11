using Microsoft.EntityFrameworkCore;
using Product.Core.Data.IRepository;
using Product.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProductEntity = Product.Core.Models.Product;

namespace Product.Persistence.Tests.ProductRepository
{
    public class ProductRepository_InsertProduct_Tests
    {
        [Fact]
        public async Task InsertProduct_InsertedWithSuccess()
        {
            var optionbuilder = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var context = new ProductDbContext(optionbuilder.Options);

            var repository = new GenericRepository<ProductEntity>(context);

            await repository.InsertAsync(new ProductEntity
            {
                Amount = 1,
                Category = "Category",
                isActive = true,
                Name = "Test",
                OwnerId = Guid.NewGuid()
            });

            Assert.Single(context.Product);
        }
    }
}

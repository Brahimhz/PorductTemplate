using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;
using Controllers = Product.API.Controllers;

namespace Product.API.Tests
{
    public class ProductController_Tests
    {
        [Fact]
        public async Task CreateProduct_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var ownerId = Guid.NewGuid();

            var input =
            new ProductInPut
            {
                Name = "name",
                Amount = 100,
                Category = "category",
                IsActive = true,
                OwnerId = ownerId
            };

            mockAppService
                .Setup(x => x.Add(It.IsAny<ProductInPut>()))
                .ReturnsAsync(
                    new ProductOutPut
                    {
                        Name = "name",
                        Amount = 100,
                        Category = "category",
                        IsActive = true,
                        OwnerId = ownerId
                    }
                );


            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);


            // Act
            var result = await controller.CreateProduct(input);

            // Assert
            Assert.IsType<OkObjectResult>(result);

        }


        [Fact]
        public async Task ModifyProduct_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);


            var productId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            var input =
            new ProductInPut
            {
                Name = "name",
                Amount = 100,
                Category = "category",
                IsActive = true,
                OwnerId = ownerId
            };

            mockAppService
                .Setup(x => x.Modify(productId, It.IsAny<ProductInPut>()))
                .ReturnsAsync(
                    new ProductOutPut
                    {
                        Name = "name",
                        Amount = 100,
                        Category = "category",
                        IsActive = true,
                        OwnerId = ownerId
                    }
                );

            // Act
            var result = await controller.ModifyProduct(productId, input);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task DeleteProduct_ReturnsOkResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var productId = Guid.NewGuid();

            mockAppService
                .Setup(x => x.Delete(productId))
                .ReturnsAsync(productId);

            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result = await controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task GetProductById_ReturnsOkResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var productId = Guid.NewGuid();

            mockAppService
                .Setup(x => x.GetById(productId))
                .ReturnsAsync(new ProductOutPut
                {
                    Name = "name",
                    Amount = 100,
                    Category = "category",
                    IsActive = true,
                    OwnerId = new Guid()
                });

            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);


            // Act
            var result = await controller.GetProductById(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var list = new List<ProductOutPutList>
            {
                new ProductOutPutList
                {
                    Name = "name",
                    Amount = 100,
                    Category = "category",
                    IsActive = true,
                    OwnerId = Guid.NewGuid()
                }
            };

            // Fix the setup by using ReturnsAsync directly on the Task
            mockAppService
                .Setup(x => x.GetAll())
                .ReturnsAsync(list);

            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result = await controller.GetProducts();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCategories_ReturnsOkResultWithCategories()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result = controller.GetCategories();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<List<string>>(okObjectResult.Value);
            Assert.NotEmpty(categories);
            // Add more specific assertions based on your actual implementation
        }
    }
}
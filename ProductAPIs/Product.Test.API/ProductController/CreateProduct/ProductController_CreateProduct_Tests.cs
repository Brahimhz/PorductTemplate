using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controllers = Product.API.Controllers;


namespace Product.Test.API.ProductController.CreateProduct
{
    public class ProductController_CreateProduct_Tests
    {
        [Fact]
        public async Task CreateProduct_ValidInput_ReturnsOkObjectResult()
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

            var output =
                    new ProductOutPut
                    {
                        Name = "name",
                        Amount = 100,
                        Category = "category",
                        IsActive = true,
                        OwnerId = ownerId
                    };

            mockAppService
                .Setup(x => x.Add(It.IsAny<ProductInPut>()))
                .ReturnsAsync(output);


            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateProduct(input);

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(output);
        }




        [Fact]
        public async Task CreateProduct_InvalidInput_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var mockAppService = new Mock<IProductAppService>();
            var mockLogger = new Mock<ILogger<Controllers.ProductController>>();

            var ownerId = Guid.NewGuid();

            var input1 =
            new ProductInPut
            {
                Name = "",
                Amount = 100,
                Category = "category",
                IsActive = true,
                OwnerId = ownerId
            };

            var input2 =
            new ProductInPut
            {
                Name = "name",
                Amount = 0,
                Category = "category",
                IsActive = true,
                OwnerId = ownerId
            };

            var input3 =
            new ProductInPut
            {
                Name = "name",
                Amount = 100,
                Category = null,
                IsActive = true,
                OwnerId = ownerId
            };


            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result_input1 = await controller.CreateProduct(input1);
            var result_input2 = await controller.CreateProduct(input2);
            var result_input3 = await controller.CreateProduct(input3);

            // Assert
            result_input1.Should().BeOfType<BadRequestResult>();
            result_input2.Should().BeOfType<BadRequestResult>();
            result_input3.Should().BeOfType<BadRequestResult>();

        }


        [Fact]
        public async Task CreateProduct_AppServiceFailure_ReturnsBadRequestObjectResult()
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
                .Setup(x => x.Add(input))
                .ReturnsAsync((ProductOutPut?)null);


            var controller = new Controllers.ProductController(mockAppService.Object, mockLogger.Object);

            // Act
            var result = await controller.CreateProduct(input);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }


    }





}

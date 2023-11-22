using MediatR;
using ProductEntity = Product.Core.Models;

namespace Product.Core.Infrastructure.Commands
{
    public record AddProductCommand(ProductEntity.Product input) : IRequest<Task>;
    public record ModifyProductCommand(ProductEntity.Product input) : IRequest<Unit>;
    public record DeleteProductCommand(ProductEntity.Product input) : IRequest<Task>;
}

using MediatR;
using System.Linq.Expressions;
using ProductEntity = Product.Core.Models;


namespace Product.Core.Infrastructure.Queries
{
    public record GetProductByPropQuery(Expression<Func<ProductEntity.Product, bool>>? condition) : IRequest<ProductEntity.Product>;
    public record GetProductListQuery(Expression<Func<ProductEntity.Product, bool>>? condition) : IRequest<IEnumerable<ProductEntity.Product>>;
}

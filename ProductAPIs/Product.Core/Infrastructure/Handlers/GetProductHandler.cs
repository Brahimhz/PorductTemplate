using MediatR;
using Product.Core.Data.IRepository;
using Product.Core.Infrastructure.Queries;
using ProductEntity = Product.Core.Models;

namespace Product.Core.Infrastructure.Handlers
{
    public class GetProductByPropHandler : IRequestHandler<GetProductByPropQuery, ProductEntity.Product?>
    {
        private readonly IGenericRepository<ProductEntity.Product> _repository;

        public GetProductByPropHandler(IGenericRepository<ProductEntity.Product> repository)
        {
            _repository = repository;
        }
        public Task<ProductEntity.Product?> Handle(GetProductByPropQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetByPropAsync(request.condition);
        }
    }

    public class GetProductListHandler : IRequestHandler<GetProductListQuery, IEnumerable<ProductEntity.Product>>
    {
        private readonly IGenericRepository<ProductEntity.Product> _repository;

        public GetProductListHandler(IGenericRepository<ProductEntity.Product> repository)
        {
            _repository = repository;
        }
        public Task<IEnumerable<ProductEntity.Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            return _repository.WhereAsync(request.condition);
        }
    }
}

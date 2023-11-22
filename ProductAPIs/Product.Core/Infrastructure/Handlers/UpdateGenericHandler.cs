using MediatR;
using Product.Core.Data.IRepository;
using Product.Core.Infrastructure.Commands;
using ProductEntity = Product.Core.Models;

namespace Product.Core.Infrastructure.Handlers
{
    public class AddProductHandler : IRequestHandler<AddProductCommand, Task>
    {
        private readonly IGenericRepository<ProductEntity.Product> _repository;

        public AddProductHandler(IGenericRepository<ProductEntity.Product> repository)
        {
            _repository = repository;
        }

        public Task<Task> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.InsertAsync(request.input));
        }
    }

    public class ModifyProductHandler : IRequestHandler<ModifyProductCommand, Unit>
    {
        private readonly IGenericRepository<ProductEntity.Product> _repository;

        public ModifyProductHandler(IGenericRepository<ProductEntity.Product> repository)
        {
            _repository = repository;
        }

        public Task<Unit> Handle(ModifyProductCommand request, CancellationToken cancellationToken)
        {
            _repository.Update(request.input);
            return Task.FromResult(Unit.Value);
        }
    }


    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Task>
    {
        private readonly IGenericRepository<ProductEntity.Product> _repository;

        public DeleteProductHandler(IGenericRepository<ProductEntity.Product> repository)
        {
            _repository = repository;
        }

        public Task<Task> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.Remove(request.input.Id));
        }
    }
}

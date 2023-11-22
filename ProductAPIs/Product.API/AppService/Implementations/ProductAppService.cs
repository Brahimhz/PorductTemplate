using AutoMapper;
using MediatR;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;
using Product.Core.Infrastructure.Commands;
using Product.Core.Infrastructure.Queries;
using ProductEntity = Product.Core.Models;

namespace Product.API.AppService.Implementations
{
    public class ProductAppService : IProductAppService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ProductAppService(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ProductOutPut> GetById(Guid id)
            => _mapper.Map<ProductEntity.Product?, ProductOutPut>
            (await _mediator.Send(new GetProductByPropQuery(e => e.Id == id)));
        public async Task<List<ProductOutPutList>> GetAll()
            => _mapper.Map<List<ProductEntity.Product>, List<ProductOutPutList>>
                ((List<ProductEntity.Product>)
                await _mediator.Send(new GetProductListQuery(null)));

        public async Task<ProductOutPut> Add(ProductInPut entityResource)
        {
            var entity = _mapper.Map<ProductInPut, ProductEntity.Product>(entityResource);

            entity.CreationDate = DateTime.Now;
            entity.LastUpdateDate = DateTime.Now;

            await _mediator.Send(new AddProductCommand(entity));

            await _mediator.Send(new UnitOfWorkCommand());

            return _mapper.Map<ProductEntity.Product, ProductOutPut>(entity);
        }

        public async Task<ProductOutPut> Modify(Guid id, ProductInPut entity)
        {
            var oldEntity = await _mediator.Send(new GetProductByPropQuery(e => e.Id == id));

            if (oldEntity == null)
                return _mapper.Map<ProductEntity.Product?, ProductOutPut>(oldEntity);


            _mapper.Map(entity, oldEntity);

            oldEntity.LastUpdateDate = DateTime.Now;
            await _mediator.Send(new ModifyProductCommand(oldEntity));
            await _mediator.Send(new UnitOfWorkCommand());


            await Task.Delay(TimeSpan.FromSeconds(3));


            return _mapper.Map<ProductEntity.Product?, ProductOutPut>
                (await _mediator.Send(new GetProductByPropQuery(e => e.Id == id)));
        }

        public async Task<Guid?> Delete(Guid id)
        {
            var deleteEntity = await _mediator.Send(new GetProductByPropQuery(e => e.Id == id));

            if (deleteEntity == null)
                return null;

            await _mediator.Send(new DeleteProductCommand(deleteEntity));
            await _mediator.Send(new UnitOfWorkCommand());
            return id;
        }
    }
}

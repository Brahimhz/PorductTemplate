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
        private readonly ILogger<ProductAppService> _logger;

        public ProductAppService(IMapper mapper, IMediator mediator, ILogger<ProductAppService> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ProductOutPut> GetById(Guid id)
            => _mapper.Map<ProductEntity.Product?, ProductOutPut>
            (await _mediator.Send(new GetProductByPropQuery(e => e.Id == id)));
        public async Task<List<ProductOutPutList>> GetAll()
            => _mapper.Map<List<ProductEntity.Product>, List<ProductOutPutList>>
                ((List<ProductEntity.Product>)
                await _mediator.Send(new GetProductListQuery(null)));

        public async Task<ProductOutPut?> Add(ProductInPut entityResource)
        {
            ProductEntity.Product entity = new();
            try
            {
                entity = _mapper.Map<ProductInPut, ProductEntity.Product>(entityResource);
            }
            catch (Exception e)
            {
                _logger.LogError("Error During the mapping :" + e.Message);
                return null;
            }

            entity.CreationDate = DateTime.Now;
            entity.LastUpdateDate = DateTime.Now;


            try
            {
                await _mediator.Send(new AddProductCommand(entity));

                await _mediator.Send(new UnitOfWorkCommand());

                try
                {
                    return _mapper.Map<ProductEntity.Product, ProductOutPut>(entity);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error During the mapping :" + e.Message);
                    return null;
                }

            }
            catch (Exception e)
            {
                // Add Product failed
                _logger.LogError("Add Product proccess Error :" + e.Message);
                return null;
            }

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
            try
            {
                var deleteEntity = await _mediator.Send(new GetProductByPropQuery(e => e.Id == id));

                if (deleteEntity == null)
                    return null;

                await _mediator.Send(new DeleteProductCommand(deleteEntity));
                await _mediator.Send(new UnitOfWorkCommand());
                return id;
            }
            catch (Exception e)
            {
                // Add Product failed
                _logger.LogError("Delete Product proccess Error :" + e.Message);
                return null;
            }

        }
    }
}

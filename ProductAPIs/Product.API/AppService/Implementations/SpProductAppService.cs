using AutoMapper;
using Product.API.AppService.Contracts;
using Product.API.AppService.Dtos.Product;
using Product.Core.Data.IRepository;
using ProductEntity = Product.Core.Models;


namespace Product.API.AppService.Implementations
{
    public class SpProductAppService : ISpProductAppService
    {
        private readonly IMapper _mapper;
        private readonly ISpProductRepository _repository;
        private readonly ILogger<ProductAppService> _logger;
        public SpProductAppService(IMapper mapper, ISpProductRepository repository, ILogger<ProductAppService> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }


        public async Task<ProductOutPut> GetById(Guid id)
            => _mapper.Map<ProductEntity.Product?, ProductOutPut>
            (await _repository.GetByIdAsync(id));
        public async Task<List<ProductOutPutList>> GetAll()
            => _mapper.Map<List<ProductEntity.Product>, List<ProductOutPutList>>
                (await _repository.GetAllAsync());

        public async Task<int> Add(ProductInPut input)
        {
            ProductEntity.Product entity = new();
            try
            {
                entity = _mapper.Map<ProductInPut, ProductEntity.Product>(input);
            }
            catch (Exception e)
            {
                _logger.LogError("Error During the mapping :" + e.Message);
                return 0;
            }

            return await _repository.InsertAsync(entity);


        }

        public async Task<int> Modify(Guid id, ProductInPut input)
        {
            ProductEntity.Product entity = new();
            try
            {
                entity = _mapper.Map<ProductInPut, ProductEntity.Product>(input);
            }
            catch (Exception e)
            {
                _logger.LogError("Error During the mapping :" + e.Message);
                return 0;
            }

            return await _repository.UpdateAsync(id, entity);

        }

        public async Task<int> Delete(Guid id)
            => await _repository.RemoveAsync(id);
    }
}

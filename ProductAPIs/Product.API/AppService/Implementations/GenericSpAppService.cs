using AutoMapper;
using Product.API.AppService.Contracts;
using Product.Core.Data.IRepository;
using Product.Core.Models;

namespace Product.API.AppService.Implementations
{
    public class GenericSpAppService<T, Tinsert, TOutPut, TOutPutList, TInPut>
        : IGenericSpAppService<T, Tinsert, TOutPut, TOutPutList, TInPut>
        where T : class, BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IGenericSpRepository<T, Tinsert> _repository;
        private readonly ILogger<GenericSpAppService<T, Tinsert, TOutPut, TOutPutList, TInPut>> _logger;
        public GenericSpAppService(
            IMapper mapper,
            IGenericSpRepository<T, Tinsert> repository,
            ILogger<GenericSpAppService<T, Tinsert, TOutPut, TOutPutList, TInPut>> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }


        public async Task<TOutPut> GetById(Guid id)
            => _mapper.Map<T, TOutPut>
            (await _repository.GetByIdAsync(id));
        public async Task<List<TOutPutList>> GetAll()
            => _mapper.Map<List<T>, List<TOutPutList>>
                (await _repository.GetAllAsync());

        public async Task<int> Add(TInPut input)
        {
            try
            {
                var entity = _mapper.Map<TInPut, Tinsert>(input);

                return await _repository.InsertAsync(entity);
            }
            catch (Exception e)
            {
                _logger.LogError("Error During the mapping :" + e.Message);
                return 0;
            }

        }

        public async Task<int> Modify(Guid id, TInPut input)
        {
            try
            {
                var entity = _mapper.Map<TInPut, Tinsert>(input);
                return await _repository.UpdateAsync(id, entity);

            }
            catch (Exception e)
            {
                _logger.LogError("Error During the mapping :" + e.Message);
                return 0;
            }


        }

        public async Task<int> Delete(Guid id)
            => await _repository.RemoveAsync(id);
    }



}

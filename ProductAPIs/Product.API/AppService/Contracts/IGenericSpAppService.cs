using Product.Core.Models;

namespace Product.API.AppService.Contracts
{
    public interface IGenericSpAppService<T, Tinsert, TOutPut, TOutPutList, TInPut> where T : class, BaseEntity
    {
        Task<TOutPut> GetById(Guid id);
        Task<List<TOutPutList>> GetAll();

        Task<int> Add(TInPut entity);
        Task<int> Modify(Guid id, TInPut input);
        Task<int> Delete(Guid id);
    }
}

using Product.Core.Models;

namespace Product.Core.Data.IRepository
{
    public interface IGenericSpRepository<T, Tinsert> where T : class, BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<int> InsertAsync(Tinsert entity);
        Task<int> UpdateAsync(Guid id, Tinsert entity);
        Task<int> RemoveAsync(Guid id);
    }
}

using Product.Core.Models;
using System.Linq.Expressions;

namespace Product.Core.Data.IRepository
{
    public interface IGenericRepository<T> where T : class, BaseEntity
    {
        Task<T?> GetByPropAsync(Expression<Func<T, bool>>? condition);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>>? predicate);
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        Task Remove(Guid id);
    }
}

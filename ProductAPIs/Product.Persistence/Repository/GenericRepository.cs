using Microsoft.EntityFrameworkCore;
using Product.Core.Data.IRepository;
using Product.Core.Models;
using System.Linq.Expressions;

namespace Product.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly ProductDbContext _context;

        public GenericRepository(ProductDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByPropAsync(Expression<Func<T, bool>>? condition)
        {
            return
                condition is not null
                ? await _dbSet.FirstOrDefaultAsync(condition)
                : await _dbSet.FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>>? predicate)
        {
            return
                predicate is not null
                ? await _dbSet.Where(predicate).ToListAsync()
                : await _dbSet.ToListAsync();
        }

        public virtual async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            _context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task Remove(Guid id)
        {
            var entity = await GetByPropAsync(e => e.Id == id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}

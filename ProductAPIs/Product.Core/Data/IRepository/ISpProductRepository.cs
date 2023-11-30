using ProductE = Product.Core.Models;

namespace Product.Core.Data.IRepository
{
    public interface ISpProductRepository
    {
        Task<ProductE.Product?> GetByIdAsync(Guid id);
        Task<List<ProductE.Product>> GetAllAsync();
        Task<int> InsertAsync(ProductE.Product entity);
        Task<int> UpdateAsync(Guid id, ProductE.Product entity);
        Task<int> RemoveAsync(Guid id);
    }
}

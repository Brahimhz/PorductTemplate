using Product.API.AppService.Dtos.Product;

namespace Product.API.AppService.Contracts
{
    public interface ISpProductAppService
    {
        Task<ProductOutPut> GetById(Guid id);
        Task<List<ProductOutPutList>> GetAll();

        Task<int> Add(ProductInPut entity);
        Task<int> Modify(Guid id, ProductInPut input);
        Task<int> Delete(Guid id);
    }
}

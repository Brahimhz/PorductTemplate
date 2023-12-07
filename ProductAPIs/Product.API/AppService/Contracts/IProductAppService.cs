using Product.API.AppService.Dtos.Product;

namespace Product.API.AppService.Contracts
{
    public interface IProductAppService
    {
        Task<ProductOutPut> GetById(Guid id);
        Task<List<ProductOutPutList>> GetAll();

        Task<ProductOutPut?> Add(ProductInPut input);
        Task<ProductOutPut> Modify(Guid id, ProductInPut input);
        Task<Guid?> Delete(Guid id);
    }
}

namespace Product.Core.Data.IRepository
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}

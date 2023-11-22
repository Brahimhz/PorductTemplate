using Product.Core.Data.IRepository;

namespace Product.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProductDbContext context;

        public UnitOfWork(ProductDbContext context)
        {
            this.context = context;
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}

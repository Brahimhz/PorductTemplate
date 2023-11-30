using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Core.Configuration;
using Product.Core.Data.IRepository;
using ProductE = Product.Core.Models;

namespace Product.Persistence.Repository
{
    public class SpProductRepository : ISpProductRepository
    {


        private readonly DbSet<ProductE.Product> _dbSet;
        private readonly ProductDbContext _context;
        private readonly StoredProducerTitles _storedProducer;

        public SpProductRepository(ProductDbContext context, IOptions<StoredProducerTitles> options)
        {
            _context = context;
            _dbSet = context.Set<ProductE.Product>();

            _storedProducer = options.Value;
        }

        public async Task<ProductE.Product?> GetByIdAsync(Guid id)
        {
            //Check the existance of StoredProcuder

            if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.GetById) ||
                StoredProcedureExists(_storedProducer.GetById)
              )
                return null;

            var result =
                await _dbSet
                .FromSqlRaw(
                    "EXEC " + _storedProducer.GetById + " @ProductId",
                    new SqlParameter("@ProductId", id))
                .ToListAsync();

            if (result is null) return null;
            if (!result.Any()) return null;

            return result.FirstOrDefault();
        }

        public async Task<List<ProductE.Product>> GetAllAsync()
        {
            if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.GetAllRecords) ||
                StoredProcedureExists(_storedProducer.GetAllRecords)
              )
                return null;

            var result = await _dbSet.FromSqlRaw(_storedProducer.GetAllRecords).ToListAsync();
            return result;
        }

        public async Task<int> InsertAsync(ProductE.Product entity)
        {
            if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.InsertProduct) ||
                StoredProcedureExists(_storedProducer.InsertProduct)
              )
                return 0;


            return
                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {_storedProducer.InsertProduct} @Name, @Category, @Amount, @IsActive",
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@Category", entity.Category),
                    new SqlParameter("@Amount", entity.Amount),
                    new SqlParameter("@IsActive", entity.isActive)
                );
        }



        public async Task<int> UpdateAsync(Guid id, ProductE.Product entity)
        {
            if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.UpdateProductById) ||
                StoredProcedureExists(_storedProducer.UpdateProductById)
              )
                return 0;

            return
                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {_storedProducer.UpdateProductById} @Id, @NewName, @NewCategory, @NewAmount, @NewIsActive",
                    new SqlParameter("@Id", id),
                    new SqlParameter("@NewName", entity.Name),
                    new SqlParameter("@NewCategory", entity.Category),
                    new SqlParameter("@NewAmount", entity.Amount),
                    new SqlParameter("@NewIsActive", entity.isActive)
                );
        }





        public async Task<int> RemoveAsync(Guid id)
        {
            if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.DeleteProductById) ||
                StoredProcedureExists(_storedProducer.DeleteProductById)
              )
                return 0;

            return
                await _context.Database.ExecuteSqlRawAsync(
                    $"EXEC {_storedProducer.DeleteProductById} @ProductId",
                    new SqlParameter("@ProductId", id)
                );
        }



        private bool StoredProcedureExists(string storedProcedureName)
        {
            var sql = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = '{storedProcedureName}'";

            var result = _context.Database.ExecuteSqlRaw(sql);

            return result > 0;
        }
    }
}

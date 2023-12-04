using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.Core.Configuration;
using Product.Core.Data.IRepository;
using Product.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Persistence.Repository
{
    public class GenericSpRepository<T, Tinsert> : IGenericSpRepository<T, Tinsert> where T : class, BaseEntity
    {
        private readonly DbSet<T> _dbSet;
        private readonly ProductDbContext _context;
        private readonly ILogger<GenericSpRepository<T, Tinsert>> _logger;
        private readonly GenericStoredProducerTitles _storedProducer;

        private readonly string _tableName;


        public GenericSpRepository(
            ProductDbContext context,
            IOptions<GenericStoredProducerTitles> options,
            ILogger<GenericSpRepository<T, Tinsert>> logger)
        {
            _logger = logger;

            _context = context;
            _dbSet = context.Set<T>();

            _storedProducer = options.Value;

            //Get Entity Table Name
            Type type = typeof(T);
            var attribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));

            _tableName = attribute is not null ? attribute.Name : type.Name;


        }


        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                //Check the existance of StoredProcuder
                if (
                    _storedProducer is null ||
                    string.IsNullOrEmpty(_storedProducer.GetById) ||
                    StoredProcedureExists(_storedProducer.GetById)
                )
                    return null;


                var sql = $"EXEC {_storedProducer.GetById} @TableName, @Id";

                var parameters = new object[] { new SqlParameter("@TableName", _tableName), new SqlParameter("@Id", id) };

                var result = await _dbSet.FromSqlRaw(sql, parameters).ToListAsync();

                if (result is null || !result.Any()) return null;

                return result.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while try to get {_tableName} by Id: {ex.Message}");
                return null;
            }

        }
        public virtual async Task<List<T>> GetAllAsync()
        {
            try
            {
                if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.GetAll) ||
                StoredProcedureExists(_storedProducer.GetAll)
            )
                    return null;

                var sql = $"EXEC {_storedProducer.GetAll} @TableName = {_tableName}";

                var result = await _dbSet.FromSqlRaw(sql).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while try to get all {_tableName} : {ex.Message}");
                return null;
            }

        }
        public virtual async Task<int> InsertAsync(Tinsert entity)
        {
            try
            {
                if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.Insert) ||
                StoredProcedureExists(_storedProducer.Insert)
              )
                    return 0;

                var columnValues = GetColumnValues(entity);

                string columns = string.Join(", ", columnValues.Keys);
                string values = string.Join(", ", columnValues.Values.Select(value => $"@{value}"));

                var sql = GenerateSql(columnValues, _storedProducer.Insert);
                var SqlParamteres = GetSqlParametersFromDictionary(columnValues);

                return await _context.Database.ExecuteSqlRawAsync(sql, SqlParamteres);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while try to insert {_tableName}: {ex.Message}");
                return 0;
            }

        }
        public virtual async Task<int> UpdateAsync(Guid id, Tinsert entity)
        {
            try
            {
                if (
                _storedProducer is null ||
                string.IsNullOrEmpty(_storedProducer.UpdateById) ||
                StoredProcedureExists(_storedProducer.UpdateById)
              )
                    return 0;

                var setValues = GetColumnValues(entity);

                string setClause = string.Join(", ", setValues.Select(kv => $"{kv.Key} = @{kv.Key}"));

                string sql = $"EXEC {_storedProducer.UpdateById} @TableName = {_tableName}, @SetClause = '{setClause}', @Id = '{id}'";

                return await _context.Database.ExecuteSqlRawAsync(sql, setValues.Select(kv => new SqlParameter($"@{kv.Key}", kv.Value)).ToArray());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while try to update {_tableName} by Id: {ex.Message}");
                return 0;
            }

        }
        public virtual async Task<int> RemoveAsync(Guid id)
        {
            try
            {
                if (
                    _storedProducer is null ||
                    string.IsNullOrEmpty(_storedProducer.DeleteById) ||
                    StoredProcedureExists(_storedProducer.DeleteById)
                )
                    return 0;

                string sql = $"EXEC {_storedProducer.DeleteById} @TableName = {_tableName}, @Id = {id}";

                return
                    await _context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while try to delete {_tableName} by Id: {ex.Message}");
                return 0;
            }

        }


        private bool StoredProcedureExists(string storedProcedureName)
        {
            var sql = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME = '{storedProcedureName}'";

            var result = _context.Database.ExecuteSqlRaw(sql);

            return result > 0;
        }
        private Dictionary<string, object> GetColumnValues(Tinsert entity)
        {
            var columnValues = new Dictionary<string, object>();

            var properties = typeof(Tinsert).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                columnValues.Add(property.Name, value);
            }

            return columnValues;
        }


        private SqlParameter[] GetSqlParametersFromDictionary(Dictionary<string, object> parameterDictionary)
        {
            List<SqlParameter> sqlParameters =
                new List<SqlParameter>() { new SqlParameter("@TableName", _tableName) };

            foreach (var kvp in parameterDictionary)
            {
                sqlParameters.Add(new SqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value));
            }

            return sqlParameters.ToArray();
        }

        private string GenerateSql(Dictionary<string, object> parameterDictionary, string procedureName)
        {
            string parameterNames = string.Join(", ", parameterDictionary.Keys.Select(key => $"@{key}"));

            return $"EXEC {procedureName} @TableName = '{_tableName}', {parameterNames}";
        }
    }
}

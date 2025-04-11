using System.Linq.Expressions;
using IntegrationTestPoc.Util;
using IntegrationTestProc.Infra;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestPoc.Tests.Helper;

public class TestStarterHelper : IDisposable
{
    private readonly Context _dbContext;

    public TestStarterHelper(IAppConfiguration appConfiguration)
    {
        var dbContextOptions = new DbContextOptionsBuilder<Context>()
            .UseSqlServer(appConfiguration.GetSqlServerConnectionString(), sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            })
            .Options;

        _dbContext = new Context(dbContextOptions);
    }

    public void GenerateDatabase()
    {
        var createScript = _dbContext.Database.GenerateCreateScript();

        try
        {
            _dbContext.Database.ExecuteSqlRaw(createScript.Replace("GO", ""));
        }
        catch (SqlException sqlException)
        {
            Console.WriteLine(sqlException);
        }
    }

    public async Task<T?> GetEntityAsync<T>(Expression<Func<T, bool>>? filter) where T : class
    {
        var dbSet = _dbContext.Set<T>();
        var query = dbSet.AsQueryable();

        if (filter is not null)
            query = query.Where(filter).AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>>? filter) where T : class
    {
        var dbSet = _dbContext.Set<T>();
        var query = dbSet.AsQueryable();

        if (filter is not null)
            query = query.Where(filter).AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task SeedAsync<T>(T entity) where T : class
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedRangeAsync<T>(List<T> entityLst, bool enableIdentityInsert = false) where T : class
    {
        var dbSet = _dbContext.Set<T>();

        if (enableIdentityInsert)
        {
            var entityType = _dbContext.Model.FindEntityType(typeof(T));
            if (entityType == null)
                throw new InvalidOperationException($"Tipo de entidade {typeof(T).Name} não encontrado no modelo.");

            var tableName = entityType.GetTableName();

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON");
                await dbSet.AddRangeAsync(entityLst);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        else
        {
            await dbSet.AddRangeAsync(entityLst);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync<T>() where T : class
    {
        var dbSet = _dbContext.Set<T>();

        dbSet.RemoveRange(dbSet);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllTablesAsync()
    {
        foreach (var entityType in _dbContext.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName}");

                var hasIdentityColumn = entityType.GetProperties()
                    .Any(p => p.ValueGenerated == Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd);

                if (hasIdentityColumn)
                {
                    await _dbContext.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{tableName}', RESEED, 0)");
                }
            }
        }

        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}

using System.Linq.Expressions;
using LocalizationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Data.Master.Repositories;

public abstract class RepositoryBase<T> where T : Entity
{
    protected readonly MasterDbContext DbContext;

    protected RepositoryBase(MasterDbContext dbContext) => 
        DbContext = dbContext;


    public Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? predicate = null,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<T>().AsNoTracking().AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        
        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return query.ToListAsync(cancellationToken);
    }

    public Task<T?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken)
    {
        return DbContext.Set<T>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task RemoveAsync(
        long id,
        CancellationToken cancellationToken)
    {
        return DbContext
            .Set<T>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<T>().AsNoTracking().AsQueryable();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        
        return query.AnyAsync(predicate, cancellationToken);
    }
}
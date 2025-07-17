using AutoMapper;
using AutoMapper.QueryableExtensions;
using LocalizationService.Data.Extensions;
using LocalizationService.Data.Master;
using LocalizationService.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Shared;

public abstract class ListHandler<TEntity, TModel, TFilter> : IListHandler<TModel, TFilter> where TEntity : class
{
    private readonly MasterDbContext _dbContext;
    private readonly IMapper _mapper;
    
    protected ListHandler(MasterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ListResult<TModel>> GetListAsync(
        ListQuery<TFilter> listQuery,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        query = ApplyFilter(query, listQuery.Filter);

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = query.ApplyPaging(listQuery.Skip, listQuery.Take);
        
        var model = await query
            .ProjectTo<TModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ListResult<TModel>(model.ToList(), totalCount);
    }
    
    protected abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TFilter? filter);
}

public class ListHandler<TEntity, TModel> : IListHandler<TModel> where TEntity : class
{
    private readonly MasterDbContext _dbContext;
    private readonly IMapper _mapper;
    
    protected ListHandler(MasterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ListResult<TModel>> GetListAsync(
        ListQuery listQuery,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = query.ApplyPaging(listQuery.Skip, listQuery.Take);
        
        var model = await query
            .ProjectTo<TModel>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ListResult<TModel>(model.ToList(), totalCount);
    }
}
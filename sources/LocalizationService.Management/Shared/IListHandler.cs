using LocalizationService.Shared.Contracts;

namespace LocalizationService.Management.Shared;

public interface IListHandler<TModel, TFilter>
{
    Task<ListResult<TModel>> GetListAsync(
        ListQuery<TFilter> query,
        CancellationToken cancellationToken = default);
}

public interface IListHandler<TModel>
{
    Task<ListResult<TModel>> GetListAsync(
        ListQuery query,
        CancellationToken cancellationToken);
}
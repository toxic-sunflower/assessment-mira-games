using AutoMapper;
using LocalizationService.Data.Master;
using LocalizationService.Domain.Translations;
using LocalizationService.Management.Contracts.Translations;
using LocalizationService.Management.Shared;

namespace LocalizationService.Management.Translations;

public class LocalizationKeyListHandler : ListHandler<LocalizationKey, LocalizationKeyModel, string>
{
    public LocalizationKeyListHandler(MasterDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
    
    protected override IQueryable<LocalizationKey> ApplyFilter(
        IQueryable<LocalizationKey> query,
        string? filter)
    {
        return string.IsNullOrWhiteSpace(filter)
            ? query
            : query.Where(x => x.Key.StartsWith(filter));
    }
}
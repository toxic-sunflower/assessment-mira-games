using AutoMapper;
using LocalizationService.Data.Master;
using LocalizationService.Domain.Languages;
using LocalizationService.Management.Contracts.Languages;
using LocalizationService.Management.Shared;

namespace LocalizationService.Management.Languages;

public class LanguageListHandler : ListHandler<Language, LanguageModel, string>
{
    public LanguageListHandler(MasterDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

    protected override IQueryable<Language> ApplyFilter(
        IQueryable<Language> query,
        string? filter)
    {
        return string.IsNullOrWhiteSpace(filter)
            ? query
            : query.Where(x => x.Locale.StartsWith(filter) ||
                               x.DisplayName.StartsWith(filter));
    }
}
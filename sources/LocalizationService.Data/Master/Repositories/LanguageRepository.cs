using LocalizationService.Domain.Languages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LocalizationService.Data.Master.Repositories;

public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
{
    public LanguageRepository(MasterDbContext dbContext) : base(dbContext) { }


    public Task<bool> ExistsLanguageWithCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return AnyAsync(x => x.Locale == code, cancellationToken);
    }

    public async Task SaveLanguageAsync(
        Language language,
        CancellationToken cancellationToken)
    {
        var entry = DbContext.ChangeTracker
            .Entries<Language>()
            .SingleOrDefault(x => x.Entity == language);

        if (entry?.State is null or EntityState.Detached)
        {
            await DbContext.AddAsync(language, cancellationToken);
        }
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Language?> LoadLanguageAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return DbContext.Languages
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Locale == code, cancellationToken);
    }

    public Task<LanguageTranslationsCount?> GetLanguageTranslationsCountAsync(
        string locale,
        CancellationToken cancellationToken)
    {
        return DbContext.Languages
            .Where(x => x.Locale == locale)
            .Select(x => new LanguageTranslationsCount(x.Id, x.Translations.Count))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task RemoveLanguageAsync(
        long languageId,
        CancellationToken cancellationToken)
    {
        await using var tx = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        await DbContext.Translations
            .Where(x => x.Language.Id == languageId)
            .ExecuteDeleteAsync(cancellationToken);
        
        await DbContext.Languages
            .Where(x => x.Id == languageId)
            .ExecuteDeleteAsync(cancellationToken);
        
        await tx.CommitAsync(cancellationToken);
    }
}
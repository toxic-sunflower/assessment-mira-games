using LocalizationService.Domain.Translations;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Data.Master.Repositories;


public class TranslationRepository : RepositoryBase<LocalizationKeyTranslation>, ITranslationRepository
{
    public TranslationRepository(MasterDbContext dbContext) : base(dbContext) { }

    
    public Task<LocalizationKeyTranslation?> LoadTranslationAsync(
        long keyId,
        string locale,
        CancellationToken cancellationToken)
    {
        return DbContext.Translations
            .AsTracking()
            .SingleOrDefaultAsync(
                x => x.LocalizationKeyId == keyId && x.Locale == locale,
                cancellationToken);
    }

    public async Task SaveTranslationAsync(
        LocalizationKeyTranslation translation,
        CancellationToken cancellationToken)
    {
        var entry = DbContext.ChangeTracker
            .Entries<LocalizationKeyTranslation>()
            .SingleOrDefault(x => x.Entity == translation);

        if (entry?.State is null or EntityState.Detached)
        {
            await DbContext.AddAsync(translation, cancellationToken);
        }
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
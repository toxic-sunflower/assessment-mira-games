using LocalizationService.Domain.Translations;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Data.Master.Repositories;

public class LocalizationKeyRepository : RepositoryBase<LocalizationKey>, ILocalizationKeyRepository
{
    public LocalizationKeyRepository(MasterDbContext dbContext) : base(dbContext) { }
    
    public Task<bool> KeyExistsAsync(
        string key,
        CancellationToken cancellationToken)
    {
        return AnyAsync(x => x.Key == key, cancellationToken);
    }

    public async Task SaveLocalizationKeyAsync(
        LocalizationKey localizationKey,
        CancellationToken cancellationToken)
    {
        var entry = DbContext.ChangeTracker
            .Entries<LocalizationKey>()
            .SingleOrDefault(x => x.Entity == localizationKey);

        if (entry?.State is null or EntityState.Detached)
        {
            await DbContext.AddAsync(localizationKey, cancellationToken);
        }
        
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<LocalizationKey?> LoadLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        return DbContext.LocalizationKeys
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
    }

    public async Task<long?> GetKeyIdAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var result = await DbContext.LocalizationKeys
            .AsNoTracking()
            .Where(x => x.Key == key)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(cancellationToken);

        return result?.Id;
    }

    public async Task RemoveAsync(
        LocalizationKey target,
        CancellationToken cancellationToken)
    {
        await using var tx = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        
        await DbContext.Translations
            .Where(x => x.LocalizationKeyId == target.Id)
            .ExecuteDeleteAsync(cancellationToken);

        DbContext.Remove(target);
        
        await DbContext.SaveChangesAsync(cancellationToken);
        
        await tx.CommitAsync(cancellationToken);
    }
}
using LocalizationService.Data.Master;
using LocalizationService.Management.Api.Application.Translations.ChangeTracking;
using LocalizationService.Management.Contracts.LocalizationKeys;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Api.Application.Translations;

public enum RemoveAllTranslationsError
{
    KeyNotFound
}

public interface IRemoveAllKeyTranslationsHandler
{
    Task<OneOf<Success, RemoveAllTranslationsError>> RemoveAllKeyTranslationsAsync(string key,
        CancellationToken cancellationToken);
}

public class RemoveAllKeyTranslationsHandler : IRemoveAllKeyTranslationsHandler
{
    private readonly MasterDbContext _dbContext;
    private readonly IChangeTracker _changeTracker;

    public RemoveAllKeyTranslationsHandler(
        MasterDbContext masterDbContext,
        IChangeTracker changeTracker)
    {
        _dbContext = masterDbContext;
        _changeTracker = changeTracker;
    }

    
    public async Task<OneOf<Success, RemoveAllTranslationsError>> RemoveAllKeyTranslationsAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var localizationKey = await _dbContext.LocalizationKeys
            .AsTracking()
            .Include(x => x.Translations)
            .ThenInclude(x => x.Language)
            .Where(x => x.Key == key)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (localizationKey is null)
            return RemoveAllTranslationsError.KeyNotFound;

        var translations = localizationKey.Translations.ToDictionary(x => x.Language.Code, x => x.Value);

        localizationKey.Translations.Clear();

        await using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _changeTracker.TrackTranslationsChangedAsync(key, translations, newTranslations: null, cancellationToken: cancellationToken);
        
        await tx.CommitAsync(cancellationToken);
        
        return new Success();
    }
}
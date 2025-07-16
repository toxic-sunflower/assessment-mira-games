using LocalizationService.Data.Master;
using LocalizationService.Management.Api.Application.Translations.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Api.Application.Translations;

public enum RemoveLocalizationKeyError
{
    KeyNotFound
}

public interface IRemoveLocalizationKeyHandler
{
    Task<OneOf<Success, RemoveLocalizationKeyError>> RemoveLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken);
}

public class RemoveLocalizationKeyHandler : IRemoveLocalizationKeyHandler
{
    private readonly MasterDbContext _dbContext;
    private readonly IChangeTracker _changeTracker;

    public RemoveLocalizationKeyHandler(MasterDbContext dbContext, IChangeTracker changeTracker)
    {
        _dbContext = dbContext;
        _changeTracker = changeTracker;
    }

    
    public async Task<OneOf<Success, RemoveLocalizationKeyError>> RemoveLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        await using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        var target = await _dbContext.LocalizationKeys
            .Include(x => x.Translations)
            .ThenInclude(x => x.Language)
            .SingleOrDefaultAsync(x => x.Key == key, cancellationToken);
        
        if (target is null)
            return RemoveLocalizationKeyError.KeyNotFound;
 
        var translations = target.Translations.ToDictionary(x => x.Language.Code, x => x.Value);
        
        _dbContext.RemoveRange(target.Translations);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _changeTracker.TrackLocalizationKeyRemovedAsync(key, translations, cancellationToken);
        
        return new Success();
    }
}
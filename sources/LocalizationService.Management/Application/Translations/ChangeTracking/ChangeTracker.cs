using LocalizationService.Data.Master;
using LocalizationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Api.Application.Translations.ChangeTracking;

public interface IChangeTracker
{
    Task TrackLocalizationKeyCreatedAsync(
        string key,
        CancellationToken cancellationToken);

    Task TrackTranslationsChangedAsync(
        string key,
        IReadOnlyDictionary<string, string> currentTranslations,
        IReadOnlyDictionary<string, string>? newTranslations,
        CancellationToken cancellationToken);
    
    Task TrackLocalizationKeyRemovedAsync(
        string key,
        IReadOnlyDictionary<string, string> translations,
        CancellationToken cancellationToken);
}

public class ChangeTracker : IChangeTracker
{
    private readonly MasterDbContext _dbContext;

    public ChangeTracker(MasterDbContext dbContext) => 
        _dbContext = dbContext;

    
    public async Task TrackLocalizationKeyCreatedAsync(string key, CancellationToken cancellationToken)
    {
        var change = new Change
        {
            Key = key,
            CurrentState = new LocalizationKeyState(),
            NewState = new LocalizationKeyState(),
            Timestamp = DateTimeOffset.UtcNow
        };
        
        await _dbContext.AddAsync(change, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task TrackTranslationsChangedAsync(
        string key,
        IReadOnlyDictionary<string, string> currentTranslations,
        IReadOnlyDictionary<string, string>? newTranslations,
        CancellationToken cancellationToken)
    {
        var change = await _dbContext.Changes.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

        if (change is null)
        {
            change = new Change
            {
                Key = key,
                CurrentState = new LocalizationKeyState(currentTranslations),
                Timestamp = DateTimeOffset.UtcNow,
            };
            
            await _dbContext.AddAsync(change, cancellationToken);
        }

        change.NewState = new LocalizationKeyState(newTranslations);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task TrackLocalizationKeyRemovedAsync(
        string key,
        IReadOnlyDictionary<string, string> translations,
        CancellationToken cancellationToken)
    {
        var change = await _dbContext.Changes.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

        if (change is null)
        {
            change = new Change
            {
                Key = key,
                CurrentState = new LocalizationKeyState(translations),
                Timestamp = DateTimeOffset.UtcNow,
            };
            
            await _dbContext.AddAsync(change, cancellationToken);
        }

        change.NewState = default;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
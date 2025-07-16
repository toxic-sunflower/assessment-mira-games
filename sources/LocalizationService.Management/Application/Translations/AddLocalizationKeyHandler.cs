using LocalizationService.Data.Master;
using LocalizationService.Domain;
using LocalizationService.Management.Api.Application.Translations.ChangeTracking;
using LocalizationService.Management.Contracts.LocalizationKeys;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace LocalizationService.Management.Api.Application.Translations;

public enum AddLocalizationKeyError
{
    AlreadyExists
}

public interface IAddLocalizationKeyHandler
{
    Task<OneOf<LocalizationKeyModel, AddLocalizationKeyError>> AddLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken);
}

public class AddLocalizationKeyHandler : IAddLocalizationKeyHandler
{
    private readonly MasterDbContext _dbContext;
    private readonly IChangeTracker _changeTracker;

    public AddLocalizationKeyHandler(
        MasterDbContext dbContext,
        IChangeTracker changeTracker)
    {
        _dbContext = dbContext;
        _changeTracker = changeTracker;
    }


    public async Task<OneOf<LocalizationKeyModel, AddLocalizationKeyError>> AddLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        if (await _dbContext.LocalizationKeys.AnyAsync(x => x.Key == key, cancellationToken))
            return AddLocalizationKeyError.AlreadyExists;

        var localizationKey = new LocalizationKey {Key = key};

        await using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        await _dbContext.AddAsync(localizationKey, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _changeTracker.TrackLocalizationKeyCreatedAsync(
            key,
            cancellationToken);
        
        await tx.CommitAsync(cancellationToken);
        
        return new LocalizationKeyModel {Key = localizationKey.Key};
    }
}
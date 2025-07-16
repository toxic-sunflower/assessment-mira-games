using LocalizationService.Data.Master;
using LocalizationService.Domain;
using LocalizationService.Management.Api.Application.Translations.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Api.Application.Translations;

public enum UpdateLocalizationKeyTranslationError
{
    KeyNotFound,
    LanguageNotFound
}

public interface IUpdateLocalizationKeyTranslationHandler
{
    Task<OneOf<Success, UpdateLocalizationKeyTranslationError>> UpdateLocalizationKeyTranslationAsync(string key,
        string locale,
        string value,
        CancellationToken cancellationToken);
}

public class UpdateLocalizationKeyTranslationHandler : IUpdateLocalizationKeyTranslationHandler
{
    private readonly MasterDbContext _dbContext;
    private readonly IChangeTracker _changeTracker;

    public UpdateLocalizationKeyTranslationHandler(
        MasterDbContext dbContext,
        IChangeTracker changeTracker)
    {
        _dbContext = dbContext;
        _changeTracker = changeTracker;
    }


    public async Task<OneOf<Success, UpdateLocalizationKeyTranslationError>> UpdateLocalizationKeyTranslationAsync(
        string key,
        string locale,
        string value,
        CancellationToken cancellationToken)
    {
        var localizationKey = await _dbContext.LocalizationKeys
            .Include(x => x.Translations)
            .ThenInclude(x => x.Language)
            .Where(x => x.Key == key)
            .SingleOrDefaultAsync(cancellationToken);

        if (localizationKey is null)
            return UpdateLocalizationKeyTranslationError.KeyNotFound;
        
        var language = await _dbContext.Languages
            .Where(x => x.Code == locale)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(cancellationToken);

        if (language is null)
            return UpdateLocalizationKeyTranslationError.LanguageNotFound;
        
        var translations = localizationKey.Translations.ToDictionary(x => x.Language.Code, x => x.Value);
        
        var translation = await _dbContext.Translations
            .SingleOrDefaultAsync(
                x => x.LanguageId == language.Id && x.LocalizationKeyId == localizationKey.Id,
                cancellationToken);

        var newTranslations = translations.ToDictionary();
        
        if (translation is null)
        {
            translation = new LocalizationKeyTranslation
            {
                LanguageId = language.Id,
                LocalizationKeyId = localizationKey.Id
            };
            
            await _dbContext.AddAsync(translation, cancellationToken);
        }

        translation.Value = value;

        await using var tx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _changeTracker.TrackTranslationsChangedAsync(key, translations, newTranslations, cancellationToken);

        await tx.CommitAsync(cancellationToken);
        
        return new Success();
    }
}
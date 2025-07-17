using LocalizationService.Domain.Translations;
using LocalizationService.Domain.Languages;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Translations;

public interface IUpdateTranslationHandler
{
    Task<OneOf<Success, ApplicationError>> UpdateLocalizationKeyTranslationAsync(
        string key,
        string locale,
        string value,
        CancellationToken cancellationToken);
}

public class UpdateTranslationHandler : IUpdateTranslationHandler
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ILocalizationKeyRepository _localizationKeyRepository;
    private readonly ITranslationRepository _translationRepository;
    
    public UpdateTranslationHandler(
        ILanguageRepository languageRepository,
        ILocalizationKeyRepository localizationKeyRepository,
        ITranslationRepository translationRepository)
    {
        _languageRepository = languageRepository;
        _localizationKeyRepository = localizationKeyRepository;
        _translationRepository = translationRepository;
    }

    
    public async Task<OneOf<Success, ApplicationError>> UpdateLocalizationKeyTranslationAsync(
        string key,
        string locale,
        string value,
        CancellationToken cancellationToken)
    {
        if (!await _languageRepository.ExistsLanguageWithCodeAsync(locale, cancellationToken))
            return ApplicationError.LanguageNotFound;

        var localizationKeyId = await _localizationKeyRepository.GetKeyIdAsync(key, cancellationToken);
        if (localizationKeyId is not {} keyId)
            return ApplicationError.KeyNotFound;
        
        var translation = await _translationRepository.LoadTranslationAsync(key, locale, cancellationToken);

        if (translation is null)
        {
            translation = new LocalizationKeyTranslation
            {
                LocalizationKeyId = keyId,
                Locale = locale,
                Value = value
            };
        }
        
        await _translationRepository.SaveTranslationAsync(translation, cancellationToken);
        
        return new Success();
    }
}
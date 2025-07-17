namespace LocalizationService.Domain.Translations;

public interface ITranslationRepository
{
    Task<LocalizationKeyTranslation?> LoadTranslationAsync(long keyId,
        string locale,
        CancellationToken cancellationToken);
    
    Task SaveTranslationAsync(
        LocalizationKeyTranslation translation,
        CancellationToken cancellationToken);
}
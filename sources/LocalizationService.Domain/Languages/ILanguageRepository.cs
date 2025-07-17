namespace LocalizationService.Domain.Languages;

public interface ILanguageRepository
{
    Task<bool> ExistsLanguageWithCodeAsync(
        string code,
        CancellationToken cancellationToken);
    
    Task SaveLanguageAsync(
        Language language,
        CancellationToken cancellationToken);

    Task<Language?> LoadLanguageAsync(
        string code,
        CancellationToken httpContextRequestAborted);

    Task<LanguageTranslationsCount?> GetLanguageTranslationsCountAsync(
        string locale,
        CancellationToken cancellationToken);

    Task RemoveLanguageAsync(
        long languageId,
        CancellationToken cancellationToken);
}
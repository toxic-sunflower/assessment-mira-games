using OneOf;
using OneOf.Types;
using LocalizationService.Domain.Languages;

namespace LocalizationService.Management.Languages;

public interface IRenameLanguageHandler
{
    Task<OneOf<Success, ApplicationError>> RenameLanguageAsync(
        string locale,
        string displayName,
        CancellationToken cancellationToken);
}

public class RenameLanguageHandler : IRenameLanguageHandler
{
    private readonly ILanguageRepository _repository;

    public RenameLanguageHandler(ILanguageRepository languageRepository) => 
        _repository = languageRepository;

    
    public async Task<OneOf<Success, ApplicationError>> RenameLanguageAsync(
        string locale,
        string displayName,
        CancellationToken cancellationToken)
    {
        var language = await _repository.LoadLanguageAsync(locale, cancellationToken);
        
        if (language is null)
            return ApplicationError.LanguageNotFound;
        
        language.DisplayName = displayName;

        await _repository.SaveLanguageAsync(language, cancellationToken);
        
        return new Success();
    }
}
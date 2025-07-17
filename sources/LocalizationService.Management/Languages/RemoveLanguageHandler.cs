using LocalizationService.Domain.Languages;
using LocalizationService.Data.Master;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Languages;

public interface IRemoveLanguageHandler
{
    Task<OneOf<Success, ApplicationError>> RemoveLanguageAsync(
        string locale,
        CancellationToken cancellationToken);
}

public class RemoveLanguageHandler : IRemoveLanguageHandler
{
    private readonly ILanguageRepository _repository;

    public RemoveLanguageHandler(ILanguageRepository repository) => 
        _repository = repository;
    
    
    public async Task<OneOf<Success, ApplicationError>> RemoveLanguageAsync(
        string locale,
        CancellationToken cancellationToken)
    {
        var target = await _repository.GetLanguageTranslationsCountAsync(locale, cancellationToken);

        if (target is null)
            return ApplicationError.LanguageNotFound;
        
        await _repository.RemoveLanguageAsync(target, cancellationToken);
        
        return new Success();
    }
}
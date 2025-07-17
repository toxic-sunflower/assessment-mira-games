using LocalizationService.Domain.Translations;
using OneOf;
using OneOf.Types;

namespace LocalizationService.Management.Translations;

public interface IRemoveLocalizationKeyHandler
{
    Task<OneOf<Success, ApplicationError>> RemoveLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken);
}

public class RemoveLocalizationKeyHandler : IRemoveLocalizationKeyHandler
{
    private readonly ILocalizationKeyRepository _repository;

    public RemoveLocalizationKeyHandler(ILocalizationKeyRepository repository) => 
        _repository = repository;
    
    public async Task<OneOf<Success, ApplicationError>> RemoveLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var target = await _repository.LoadLocalizationKeyAsync(key, cancellationToken);
        
        if (target is null)
            return ApplicationError.KeyNotFound;

        await _repository.RemoveAsync(target, cancellationToken);
        
        return new Success();
    }
}
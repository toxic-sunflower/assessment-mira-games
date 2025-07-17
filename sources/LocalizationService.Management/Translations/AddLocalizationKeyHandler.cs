using AutoMapper;
using LocalizationService.Domain.Translations;
using LocalizationService.Management.Contracts.Translations;
using OneOf;

namespace LocalizationService.Management.Translations;

public interface IAddLocalizationKeyHandler
{
    Task<OneOf<LocalizationKeyModel, ApplicationError>> AddLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken);
}

public class AddLocalizationKeyHandler : IAddLocalizationKeyHandler
{
    private readonly ILocalizationKeyRepository _repository;
    private readonly IMapper _mapper;
    
    public AddLocalizationKeyHandler(ILocalizationKeyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<OneOf<LocalizationKeyModel, ApplicationError>> AddLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        if (await _repository.KeyExistsAsync(key, cancellationToken))
            return ApplicationError.DuplicateDefinition;

        var localizationKey = new LocalizationKey {Key = key};

        await _repository.SaveLocalizationKeyAsync(localizationKey, cancellationToken);

        return _mapper.Map<LocalizationKeyModel>(localizationKey);
    }
}
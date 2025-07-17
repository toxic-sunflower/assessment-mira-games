using AutoMapper;
using LocalizationService.Domain.Languages;
using LocalizationService.Management.Contracts.Languages;
using OneOf;

namespace LocalizationService.Management.Languages;

public interface IAddLanguageHandler
{
    Task<OneOf<LanguageModel, ApplicationError>> AddLanguageAsync(
        string locale,
        string displayName,
        CancellationToken cancellationToken);
}

public class AddLanguageHandler : IAddLanguageHandler
{
    private readonly ILanguageRepository _repository;
    private readonly IMapper _mapper;

    public AddLanguageHandler(ILanguageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<OneOf<LanguageModel, ApplicationError>> AddLanguageAsync(
        string locale,
        string displayName,
        CancellationToken cancellationToken)
    {
        if (await _repository.ExistsLanguageWithCodeAsync(locale, cancellationToken))
            return ApplicationError.DuplicateDefinition;

        var language = new Language
        {
            Locale = locale,
            DisplayName = displayName
        };
        
        await _repository.SaveLanguageAsync(language, cancellationToken);

        return _mapper.Map<LanguageModel>(language);
    }
}
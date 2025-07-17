using AutoMapper;
using LocalizationService.Domain.Translations;
using LocalizationService.Management.Contracts.Translations;

namespace LocalizationService.Management.Translations;

public class TranslationsMappingProfile : Profile
{
    public TranslationsMappingProfile()
    {
        CreateMap<LocalizationKey, LocalizationKeyModel>();
        CreateMap<LocalizationKeyTranslation, TranslationModel>();
    }
}
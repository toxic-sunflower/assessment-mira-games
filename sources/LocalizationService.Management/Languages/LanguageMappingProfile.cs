using AutoMapper;
using LocalizationService.Domain.Languages;
using LocalizationService.Management.Contracts.Languages;

namespace LocalizationService.Management.Languages;

public class LanguageMappingProfile : Profile
{
    public LanguageMappingProfile() => CreateMap<Language, LanguageModel>();
}
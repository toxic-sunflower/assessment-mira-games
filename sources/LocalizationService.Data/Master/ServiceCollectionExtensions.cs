using LocalizationService.Data.Master.Repositories;
using LocalizationService.Domain.Languages;
using LocalizationService.Domain.Translations;
using Microsoft.Extensions.DependencyInjection;

namespace LocalizationService.Data.Master;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ITranslationRepository, TranslationRepository>()
            .AddScoped<ILanguageRepository, LanguageRepository>()
            .AddScoped<ILocalizationKeyRepository, LocalizationKeyRepository>();
    }
}
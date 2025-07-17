using LocalizationService.Management.Contracts.Languages;
using LocalizationService.Management.Contracts.Translations;
using LocalizationService.Management.Languages;
using LocalizationService.Management.Shared;
using LocalizationService.Management.Translations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LocalizationService.Management;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(options => options.AddMaps(typeof(HstsServicesExtensions)));
        
        services.AddListHandler<LanguageListHandler, LanguageModel, string>();
        services.AddListHandler<LocalizationKeyListHandler, LocalizationKeyModel, string>();
        
        services.AddScoped<IAddLanguageHandler, AddLanguageHandler>();
        services.AddScoped<IRemoveLanguageHandler, RemoveLanguageHandler>();
        services.AddScoped<IRenameLanguageHandler, RenameLanguageHandler>();
        services.AddScoped<IAddLocalizationKeyHandler, AddLocalizationKeyHandler>();
        services.AddScoped<IUpdateTranslationHandler, UpdateTranslationHandler>();
        services.AddScoped<IRemoveLocalizationKeyHandler, RemoveLocalizationKeyHandler>();

        return services;
    }

    public static IServiceCollection AddListHandler<THandler, TModel, TFilter>(this IServiceCollection services)
        where THandler : class, IListHandler<TModel, TFilter>
    {
        return services.AddScoped<IListHandler<TModel, TFilter>, THandler>();
    }
    
    public static IServiceCollection AddListHandler<THandler, TModel>(this IServiceCollection services)
        where THandler : class, IListHandler<TModel>
    {
        return services.AddScoped<IListHandler<TModel>, THandler>();
    }
    
    public static IServiceCollection AddListHandlerForEntity<TEntity, TModel>(this IServiceCollection services)
        where TEntity : class
    {
        return services.AddScoped<IListHandler<TModel>, ListHandler<TEntity, TModel>>();
    }
}
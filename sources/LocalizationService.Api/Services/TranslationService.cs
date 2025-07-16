using LocalizationService.Data.Read;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Api.Services;


public interface ITranslationService
{
    Task<string?> GetTranslationAsync(string key, string locale, CancellationToken cancellationToken);
    Task<IEnumerable<KeyValuePair<string, string>>> GetTranslationsAsync(string key, CancellationToken cancellationToken);
    Task<IEnumerable<KeyValuePair<string, string>>> GetAllTranslationForLocale(string locale, CancellationToken cancellationToken);
}

public class TranslationService : ITranslationService
{
    private readonly IDbContextFactory<ReadDbContext> _dbContextFactory;

    public TranslationService(IDbContextFactory<ReadDbContext> dbContextFactory) => 
        _dbContextFactory = dbContextFactory;

    
    public async Task<string?> GetTranslationAsync(string key, string locale, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var identifier = $"{key}_{locale}";

        return await dbContext.Translations
            .AsNoTracking()
            .Where(x => x.Identifier == identifier)
            .Select(x => x.Value)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetTranslationsAsync(string key, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Translations
            .AsNoTracking()
            .Where(x => x.LocalizationKey == key)
            .OrderBy(x => x.Locale)
            .Select(x => new KeyValuePair<string, string>(x.Locale, x.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllTranslationForLocale(string locale, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.Translations
            .AsNoTracking()
            .Where(x => x.Locale == locale)
            .OrderBy(x => x.LocalizationKey)
            .Select(x => new KeyValuePair<string, string>(x.LocalizationKey, x.Value))
            .ToListAsync(cancellationToken);
    }
}
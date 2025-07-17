namespace LocalizationService.Domain.Translations;

public interface ILocalizationKeyRepository
{
    Task<bool> KeyExistsAsync(
        string key,
        CancellationToken cancellationToken);
    
    Task SaveLocalizationKeyAsync(
        LocalizationKey localizationKey,
        CancellationToken cancellationToken);
    
    Task<LocalizationKey?> LoadLocalizationKeyAsync(
        string key,
        CancellationToken cancellationToken);
    
    Task<long?> GetKeyIdAsync(
        string key,
        CancellationToken cancellationToken);
    
    Task RemoveAsync(
        LocalizationKey target,
        CancellationToken cancellationToken);
}
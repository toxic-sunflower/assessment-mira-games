namespace LocalizationService.Domain;

public class LocalizationKey : Entity
{
    public required string Key { get; init; }
    
    public ICollection<LocalizationKeyTranslation> Translations { get; init; } = [];
}
using LocalizationService.Domain.Languages;

namespace LocalizationService.Domain.Translations;

public class LocalizationKeyTranslation : Entity
{
    public required long LocalizationKeyId { get; init; }
    public required string Locale { get; init; }
    public required string Value { get; init; }
    
    public LocalizationKey LocalizationKey { get; } = null!;
    public Language Language { get; private set; } = null!;
}
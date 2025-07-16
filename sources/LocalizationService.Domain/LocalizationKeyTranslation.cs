namespace LocalizationService.Domain;

public class LocalizationKeyTranslation : Entity
{
    public required long LanguageId { get; init; }
    public required long LocalizationKeyId { get; init; }
    public string Value { get; set; } = null!;

    public LocalizationKey LocalizationKey { get; init; } = null!;
    public Language Language { get; init; } = null!;
}
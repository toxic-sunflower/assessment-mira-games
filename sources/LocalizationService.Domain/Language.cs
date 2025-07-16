namespace LocalizationService.Domain;

public class Language : Entity
{
    public required string Code { get; init; }
    public required string? DisplayName { get; set; }

    public IReadOnlyCollection<LocalizationKeyTranslation> Translations { get; init; } = null!;
}
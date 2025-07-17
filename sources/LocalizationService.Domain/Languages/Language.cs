using LocalizationService.Domain.Translations;

namespace LocalizationService.Domain.Languages;

public class Language : Entity
{
    public required string Locale { get; init; }
    public required string DisplayName { get; set; }

    public IReadOnlyCollection<LocalizationKeyTranslation> Translations { get; init; } = [];
}
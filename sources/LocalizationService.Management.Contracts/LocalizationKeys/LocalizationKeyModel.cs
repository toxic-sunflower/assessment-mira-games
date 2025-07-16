using LocalizationService.Management.Contracts.Translations;

namespace LocalizationService.Management.Contracts.LocalizationKeys;

public class LocalizationKeyModel
{
    public string Key { get; set; } = null!;
    public string? Note { get; init; }
    public ICollection<TranslationModel> Translations { get; init; } = [];
}
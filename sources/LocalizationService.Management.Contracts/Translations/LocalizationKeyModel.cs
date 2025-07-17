namespace LocalizationService.Management.Contracts.Translations;

public class LocalizationKeyModel
{
    public long Id { get; set; }
    public string Key { get; set; } = null!;
    public string? Note { get; init; }
    public ICollection<TranslationModel> Translations { get; init; } = [];
}
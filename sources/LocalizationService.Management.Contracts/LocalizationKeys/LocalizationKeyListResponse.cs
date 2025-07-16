namespace LocalizationService.Management.Contracts.LocalizationKeys;

public readonly record struct LocalizationKeyListResponse(
    IReadOnlyList<LocalizationKeyModel> Translations);
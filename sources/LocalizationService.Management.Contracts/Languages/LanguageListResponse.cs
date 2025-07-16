namespace LocalizationService.Management.Contracts.Languages;

public readonly record struct LanguageListResponse(IEnumerable<LanguageModel> Languages);
using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.Languages;

public readonly record struct RenameLanguageRequest(
    [property: Required, MaxLength(25)] string DisplayName);
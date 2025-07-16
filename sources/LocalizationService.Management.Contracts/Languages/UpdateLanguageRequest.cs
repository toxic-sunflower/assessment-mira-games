using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.Languages;

public readonly record struct UpdateLanguageRequest(
    [property: Required, MaxLength(25)] string DisplayName);
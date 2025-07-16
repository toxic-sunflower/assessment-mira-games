using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.Languages;

public readonly record struct AddLanguageRequest(
    [property: Required, MaxLength(5)] string Code,
    [property: Required, MaxLength(25)] string DisplayName);
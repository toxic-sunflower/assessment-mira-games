using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.Translations;

public readonly record struct AddLocalizationKeyRequest(
    [property: Required, MaxLength(50)] string Key);
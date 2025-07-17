using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.Translations;

public readonly record struct UpdateTranslationRequest(
    [property: Required] string Value);
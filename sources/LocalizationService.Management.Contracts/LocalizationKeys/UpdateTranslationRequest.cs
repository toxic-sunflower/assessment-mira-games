using System.ComponentModel.DataAnnotations;

namespace LocalizationService.Management.Contracts.LocalizationKeys;

public readonly record struct UpdateTranslationRequest(
    [property: Required] string Value);
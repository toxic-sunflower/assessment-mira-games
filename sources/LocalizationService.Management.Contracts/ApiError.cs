namespace LocalizationService.Management.Contracts;

public readonly record struct ApiError(
    string Description,
    string TraceIdentifier);
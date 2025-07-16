namespace LocalizationService.Domain;

public class Change
{
    public Guid Id { get; } = default;
    public required DateTimeOffset Timestamp { get; init; }
    public required string Key { get; init; }
    public required LocalizationKeyState CurrentState { get; init; }
    public LocalizationKeyState NewState { get; set; }
}

public readonly record struct LocalizationKeyState(
    IReadOnlyDictionary<string, string>? Translations);
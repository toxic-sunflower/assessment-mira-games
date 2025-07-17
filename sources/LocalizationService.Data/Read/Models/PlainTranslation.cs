namespace LocalizationService.Data.Read.Models;

public class Translation
{
    public required string Key { get; init; }
    public required string Locale { get; init; }
    public required string Value { get; init; }
}
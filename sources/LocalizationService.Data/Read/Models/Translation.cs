namespace LocalizationService.Data.Read.Models;

public class Translation
{
    public string Identifier { get; }
    public required string LocalizationKey { get; init; }
    public required string Locale { get; init; }
    public required string Value { get; init; }
}
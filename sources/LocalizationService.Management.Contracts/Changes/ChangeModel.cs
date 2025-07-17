using System.Text.Json;

namespace LocalizationService.Management.Contracts.Changes;

public record struct ChangeModel(
    long Id,
    string Key,
    JsonDocument? OldValue,
    JsonDocument? NewValue);
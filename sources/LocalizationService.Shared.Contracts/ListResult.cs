namespace LocalizationService.Shared.Contracts;

public readonly record struct ListResult<T>(List<T> Items, long TotalCount);
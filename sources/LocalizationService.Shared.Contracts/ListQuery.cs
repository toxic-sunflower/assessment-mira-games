namespace LocalizationService.Shared.Contracts;

public readonly struct ListQuery
{
    public int? Skip { get; init; }
    public int? Take { get; init; }
}

public readonly struct ListQuery<T>
{
    public int? Skip { get; init; }
    public int? Take { get; init; }
    public T Filter { get; init; }
}
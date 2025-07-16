namespace LocalizationService.Management.Client;

public struct ApiResult<T>
{
    private readonly T? _data;

    internal ApiResult(T? data, int httpStatusCode) => (_data, HttpStatusCode) = (data, HttpStatusCode);

    public bool IsSuccess => HttpStatusCode is > 199 and < 300;
    public int HttpStatusCode { get; }

    public bool TryGetData(out T data, out int statusCode)
    {
        statusCode = HttpStatusCode;
        
        if (_data is null)
        {
            data = default!;
            return false;
        }

        data = _data;
        
        return true;
    }
}
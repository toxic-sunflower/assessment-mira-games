using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using LocalizationService.Management.Contracts.Languages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LocalizationService.Management.Client;

public interface IManagementClient : ILanguageClient;

public class ManagementClient : IManagementClient
{
    private const string LanguagePrefix = "language";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<ManagementClientOptions> _options;
    private readonly ILogger<ManagementClient> _logger;

    public ManagementClient(
        IHttpClientFactory httpClientFactory,
        IOptions<ManagementClientOptions> options,
        ILogger<ManagementClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = options;
    }

    
    public async Task<ApiResult<LanguageListResponse>> GetLanguageListAsync(
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        return await SendAsync<LanguageListResponse>(
            HttpMethod.Get,
            $"{LanguagePrefix}?skip={skip}&take={take}",
            cancellationToken);
    }

    public async Task<ApiResult<LanguageModel>> GetLanguageAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return await SendAsync<LanguageModel>(
            HttpMethod.Get,
            $"{LanguagePrefix}/{code}",
            cancellationToken);
    }

    public Task<ApiResult<LanguageModel>> AddLanguageAsync(
        AddLanguageRequest request,
        CancellationToken cancellationToken)
    {
        return SendJsonAsync<LanguageModel, AddLanguageRequest>(
            HttpMethod.Post, 
            $"{LanguagePrefix}",
            request,
            cancellationToken);
    }

    public Task<ApiResult<LanguageModel>> UpdateLanguageAsync(
        string code,
        UpdateLanguageRequest request,
        CancellationToken cancellationToken)
    {
        return SendJsonAsync<LanguageModel, UpdateLanguageRequest>(
            HttpMethod.Patch, 
            $"{LanguagePrefix}/{code}",
            request,
            cancellationToken);
    }

    public Task<ApiResult<LanguageModel>> DeleteLanguageAsync(
        string code,
        CancellationToken cancellationToken)
    {
        return SendAsync<LanguageModel>(
            HttpMethod.Delete,
            $"{LanguagePrefix}/{code}",
            cancellationToken);
    }

    
    protected async Task<ApiResult<T>> SendJsonAsync<T, TRequest>(
        HttpMethod method,
        string path,
        TRequest payload,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, path);
        
        var body = JsonSerializer.Serialize(payload);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        
        httpRequest.Content = content;
        
        return await SendRequestAsync<T>(httpRequest, cancellationToken);
    }

    protected async Task<ApiResult<T>> SendAsync<T>(
        HttpMethod method,
        string path,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, path);
        return await SendRequestAsync<T>(httpRequest, cancellationToken);
    }
    
    protected async Task<ApiResult<T>> SendRequestAsync<T>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_options.Value.BaseUrl);
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await CreateApiResultAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending request to management API: {ErrorMessage}", ex.Message);
            throw;
        }
    }
    
    protected async Task<ApiResult<T>> CreateApiResultAsync<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var statusCode = (int) response.StatusCode;

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResult<T>(default, statusCode);
        }
        
        var data = await response.Content.ReadFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);
        
        return new ApiResult<T>(data, statusCode);
    }
}
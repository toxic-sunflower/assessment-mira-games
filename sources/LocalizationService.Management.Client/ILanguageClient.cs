using LocalizationService.Management.Contracts.Languages;

namespace LocalizationService.Management.Client;

public interface ILanguageClient
{
    Task<ApiResult<LanguageListResponse>> GetLanguageListAsync(
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);
    
    Task<ApiResult<LanguageModel>> GetLanguageAsync(
        string code,
        CancellationToken cancellationToken);

    Task<ApiResult<LanguageModel>> AddLanguageAsync(
        AddLanguageRequest request,
        CancellationToken cancellationToken);
    
    Task<ApiResult<LanguageModel>> UpdateLanguageAsync(
        string code,
        UpdateLanguageRequest request,
        CancellationToken cancellationToken);
    
    Task<ApiResult<LanguageModel>> DeleteLanguageAsync(
        string code,
        CancellationToken cancellationToken);
}
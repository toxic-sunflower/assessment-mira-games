using LocalizationService.Management.Api.Application;
using LocalizationService.Management.Api.Application.Shared;
using LocalizationService.Management.Api.Application.Translations;
using LocalizationService.Management.Api.Contracts.Translations;
using LocalizationService.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Management.Api.Api.Controllers;

[ApiController]
[Route("api/localization-key")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class LocalizationKeyController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<ListResult<LocalizationKeyModel>>(StatusCodes.Status200OK)]
    public Task<ListResult<LocalizationKeyModel>> GetList(
        [FromServices] IListHandler<LocalizationKeyModel, string> handler,
        [FromQuery] ListQuery<string> query)
    {
        return handler.GetListAsync(query, HttpContext.RequestAborted);
    }
    
    
    [HttpPost]
    [ProducesResponseType<LocalizationKeyModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LocalizationKeyModel>> AddLocalizationKey(
        [FromServices] IAddLocalizationKeyHandler handler,
        [FromBody] AddLocalizationKeyRequest request)
    {
        var result = await handler.AddLocalizationKeyAsync(request.Key, HttpContext.RequestAborted);

        return result.Match<ActionResult<LocalizationKeyModel>>(
            model => Ok(model),
            error => error switch
            {
                ApplicationError.DuplicateDefinition => Conflict(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            });
    }
  
    
    [HttpPatch("{key}/translation/{locale}")]
    public async Task<ActionResult<LocalizationKeyModel>> UpdateLocalizationKeyTranslation(
        [FromServices] IUpdateTranslationHandler handler,
        [FromRoute] string key,
        [FromRoute] string locale,
        [FromBody] UpdateTranslationRequest request)
    {
        var result = await handler.UpdateLocalizationKeyTranslationAsync(
            key,
            locale,
            request.Value,
            HttpContext.RequestAborted);

        return result.Match<ActionResult<LocalizationKeyModel>>(
            model => Ok(model),
            error => NotFound(error.ToString()));
    }
    
    
    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveLocalizationKey(
        [FromServices] IRemoveLocalizationKeyHandler handler,
        [FromRoute] string key)
    {
        var result = await handler.RemoveLocalizationKeyAsync(key, HttpContext.RequestAborted);
        
        return result.Match<ActionResult>(
            _ => Ok(),
            error => NotFound(error.ToString()));        
    }
} 
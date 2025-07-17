using AutoMapper;
using LocalizationService.Domain.Languages;
using LocalizationService.Management.Api.Application;
using LocalizationService.Management.Api.Contracts;
using LocalizationService.Management.Api.Contracts.Languages;
using LocalizationService.Management.Api.Application.Languages;
using LocalizationService.Management.Api.Application.Shared;
using LocalizationService.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Management.Api.Api.Controllers;


[ApiController]
[Route("api/language")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
public class LanguageController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<ListResult<LanguageModel>>(StatusCodes.Status200OK)]
    public Task<ListResult<LanguageModel>> GetLanguageList(
        [FromServices] IListHandler<LanguageModel, string> handler,
        [FromQuery] ListQuery<string> query)
    {
        return handler.GetListAsync(query, HttpContext.RequestAborted);
    }
    
    
    [HttpPost]
    [ProducesResponseType<LanguageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LanguageModel>> AddLanguage(
        [FromServices] IAddLanguageHandler handler,
        [FromBody] AddLanguageRequest request)
    {
        var result = await handler.AddLanguageAsync(request.Locale, request.DisplayName, HttpContext.RequestAborted);

        return result.Match<ActionResult<LanguageModel>>(
            model => Ok(model),
            error => error is ApplicationError.DuplicateDefinition
                ? Conflict()
                : StatusCode(StatusCodes.Status500InternalServerError, error.ToString()));
    }
    
    
    [HttpPatch("{code}/displayName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RenameLanguage(
        [FromServices] IRenameLanguageHandler handler,
        [FromRoute] string code,
        [FromBody] RenameLanguageRequest request) 
    {
        var result = await handler.RenameLanguageAsync(code, request.DisplayName, HttpContext.RequestAborted);
        
        return result.Match<ActionResult>(
            _ => Ok(),
            error => error is ApplicationError.LanguageNotFound
                ? NotFound()
                : StatusCode(StatusCodes.Status500InternalServerError, error.ToString()));
    }
    
    
    [HttpDelete("{code}")]
    [ProducesResponseType<LanguageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveLanguage(
        [FromServices] IRemoveLanguageHandler handler,
        [FromRoute] string code)
    {
        var result = await handler.RemoveLanguageAsync(code, HttpContext.RequestAborted);
        
        return result.Match<ActionResult>(
            _ => Ok(),
            error => error is ApplicationError.LanguageNotFound
                ? NotFound()
                : StatusCode(StatusCodes.Status500InternalServerError, error.ToString()));
    }
}
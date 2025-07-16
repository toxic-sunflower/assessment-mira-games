using LocalizationService.Data.Master;
using LocalizationService.Domain;
using LocalizationService.Management.Api.Application.Translations;
using LocalizationService.Management.Contracts.LocalizationKeys;
using LocalizationService.Management.Contracts.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Api.Api.Controllers;


[ApiController]
[Route("api/localization-key")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class LocalizationKeyController : ControllerBase
{
    private readonly MasterDbContext _dbContext;

    public LocalizationKeyController(MasterDbContext dbContext) => 
        _dbContext = dbContext;


    [HttpGet]
    [ProducesResponseType<LocalizationKeyListResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LocalizationKeyListResponse>> GetList(
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null,
        [FromQuery] string? key = null)
    {
        var query = _dbContext.LocalizationKeys
            .AsQueryable()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(key))
        {
            query = query.Where(x => x.Key.StartsWith(key));
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var results = await query
            .Include(x => x.Translations)
            .ThenInclude(x => x.Language)
            .OrderBy(x => x.Key)
            .ToListAsync(HttpContext.RequestAborted);
        
        var model = results.Select(
            x => new LocalizationKeyModel
            {
                Key = x.Key,
                Translations = x.Translations
                    .Select(t => new TranslationModel {Locale = t.Language.Code, Value = t.Value})
                    .ToList()
            });

        return new LocalizationKeyListResponse(model.ToList());
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
                AddLocalizationKeyError.AlreadyExists => Conflict(),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            });
    }
  
    
    [HttpPatch("{key}/translation/{locale}")]
    public async Task<ActionResult<LocalizationKeyModel>> UpdateLocalizationKeyTranslation(
        [FromServices] IUpdateLocalizationKeyTranslationHandler handler,
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
    
    
    [HttpDelete("{key}/translation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveAllTranslations(
        [FromServices] IRemoveAllKeyTranslationsHandler handler,
        [FromRoute] string key)
    {
        var result = await handler.RemoveAllKeyTranslationsAsync(key, HttpContext.RequestAborted);
        
        return result.Match<ActionResult>(
            _ => Ok(),
            error => NotFound(error.ToString()));
    }
} 
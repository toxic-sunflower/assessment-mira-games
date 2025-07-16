using LocalizationService.Data.Master;
using LocalizationService.Domain;
using LocalizationService.Management.Contracts.LocalizationKeys;
using LocalizationService.Management.Contracts.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Management.Api.Controllers;

[ApiController]
[Route("localization-key")]
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
        [FromBody] AddLocalizationKeyRequest request)
    {
        if (await _dbContext.LocalizationKeys.AnyAsync(x => x.Key == request.Key, HttpContext.RequestAborted))
            return Conflict();

        var localizationKey = new LocalizationKey {Key = request.Key};
        
        await _dbContext.AddAsync(localizationKey, HttpContext.RequestAborted);
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);
        
        return new LocalizationKeyModel {Key = localizationKey.Key};
    }
  
    
    [HttpPatch("{key}/translation/{locale}")]
    public async Task<ActionResult<LocalizationKeyModel>> UpdateLocalizationKeyTranslation(
        [FromRoute] string key,
        [FromRoute] string locale,
        [FromBody] UpdateTranslationRequest request)
    {
        var localizationKey = await _dbContext.LocalizationKeys
            .Where(x => x.Key == key)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(HttpContext.RequestAborted);
        
        if (localizationKey is null)
            return NotFound("Key");
        
        var language = await _dbContext.Languages
            .Where(x => x.Code == locale)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(HttpContext.RequestAborted);
        
        if (language is null)
            return NotFound("Language");
        
        var translation = await _dbContext.Translations
            .SingleOrDefaultAsync(
                x => x.LanguageId == language.Id && x.LocalizationKeyId == localizationKey.Id,
                HttpContext.RequestAborted);

        if (translation is null)
        {
            translation = new LocalizationKeyTranslation
            {
                LanguageId = language.Id,
                LocalizationKeyId = localizationKey.Id
            };
            
            await _dbContext.AddAsync(translation, HttpContext.RequestAborted);
        }

        translation.Value = request.Value;
        
        await _dbContext.SaveChangesAsync(HttpContext.RequestAborted);

        return Ok();
    }
    
    
    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocalizationKeyModel>> RemoveLocalizationKey(
        [FromRoute] string key)
    {
        var recordsCount = await _dbContext.LocalizationKeys
            .Where(x => x.Key == key)
            .ExecuteDeleteAsync(HttpContext.RequestAborted);

        return recordsCount is 0 ? NotFound() : Ok();
    }
    
    
    [HttpDelete("{key}/translation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LocalizationKeyModel>> RemoveAllTranslations(
        [FromRoute] string key)
    {
        var localizationKey = await _dbContext.LocalizationKeys
            .Where(x => x.Key == key)
            .Select(x => new {x.Id})
            .SingleOrDefaultAsync(HttpContext.RequestAborted);
        
        if (localizationKey is null)
            return NotFound();
        
        await _dbContext.Translations
            .Where(x => x.LocalizationKeyId == localizationKey.Id)
            .ExecuteDeleteAsync(HttpContext.RequestAborted);
        
        return Ok();
    }
} 
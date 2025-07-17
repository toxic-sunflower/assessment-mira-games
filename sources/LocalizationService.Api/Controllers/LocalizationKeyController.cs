using LocalizationService.Data.Extensions;
using LocalizationService.Data.Read;
using LocalizationService.Data.Read.Models;
using LocalizationService.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Api.Controllers;

[ApiController]
[Route("api/localization-key")]
[Produces("application/json")]
public class LocalizationKeyController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ListResult<string>> GetAvailableKeys(
        [FromServices] ReadDbContext readDbContext,
        [FromQuery] ListQuery query)
    {
        var languages = await readDbContext.Translations
            .Select(x => x.Key)
            .Distinct()
            .ApplyPaging(query.Skip, query.Take)
            .ToListAsync(HttpContext.RequestAborted);

        return new ListResult<string>(languages, languages.Count);
    }
    
    [HttpGet("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IReadOnlyDictionary<string, string>> GetTranslationsForKey(
        [FromServices] ReadDbContext readDbContext,
        [FromRoute] string key)
    {
        return await readDbContext.Translations
            .Where(x => x.Key == key)
            .ToDictionaryAsync(x => x.Locale, x => x.Value);
    }
    
    [HttpGet("{key}/translation/{locale}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> GetTranslation(
        [FromServices] ReadDbContext dbContext,
        [FromRoute] string key,
        [FromRoute] string locale)
    {
        var result = await dbContext.Translations
            .Where(x => x.Key == key && x.Locale == locale)
            .Select(x => x.Value)
            .SingleOrDefaultAsync(HttpContext.RequestAborted);
        
        return result is null
            ? NotFound()
            : Content(result, "text/plain");
    }
}
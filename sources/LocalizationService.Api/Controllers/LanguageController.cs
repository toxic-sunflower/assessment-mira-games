using LocalizationService.Data.Read;
using LocalizationService.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalizationService.Api.Controllers;

[ApiController]
[Route("api/language")]
[Produces("application/json")]
public class LanguageController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ListResult<string>> GetAvailableLanguages(
        [FromServices] ReadDbContext readDbContext)
    {
        var languages = await readDbContext.Translations
            .Select(x => x.Locale)
            .Distinct()
            .ToListAsync(HttpContext.RequestAborted);

        return new ListResult<string>(languages, languages.Count);
    }
    
    [HttpGet("{locale}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IReadOnlyDictionary<string, string>> GetAvailableLanguages(
        [FromServices] ReadDbContext readDbContext,
        [FromRoute] string locale)
    {
        return await readDbContext.Translations
            .Where(x => x.Locale == locale)
            .ToDictionaryAsync(
                x => x.Key,
                x => x.Value,
                HttpContext.RequestAborted);
    }
}
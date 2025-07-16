using LocalizationService.Data.Master;
using LocalizationService.Domain;
using LocalizationService.Management.Contracts.Languages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiError = LocalizationService.Management.Contracts.ApiError;

namespace LocalizationService.Management.Api.Api.Controllers;

[ApiController]
[Route("api/language")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
public class LanguageController : ControllerBase
{
    private readonly MasterDbContext _masterDbContext;

    public LanguageController(MasterDbContext masterDbContext) =>
        _masterDbContext = masterDbContext;


    [HttpGet]
    [ProducesResponseType<LanguageListResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<LanguageListResponse>> GetLanguageList(
        [FromQuery] int? skip = null,
        [FromQuery] int? take = null)
    {
        var query = _masterDbContext.Languages.AsQueryable();

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        
        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        var languages = await query
            .Select(x => new LanguageModel(x.Id, x.Code, x.DisplayName))
            .ToListAsync(HttpContext.RequestAborted);
        
        return new LanguageListResponse(languages);
    }
    
    
    [HttpPost]
    [ProducesResponseType<LanguageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LanguageModel>> AddLanguage(
        [FromBody] AddLanguageRequest request) 
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        if (await _masterDbContext.Languages.AnyAsync(x => x.Code == request.Code, HttpContext.RequestAborted))
        {
            return Conflict();
        }

        var language = new Language
        {
            Code = request.Code,
            DisplayName = request.DisplayName
        };
        
        await _masterDbContext.AddAsync(language, HttpContext.RequestAborted);
        await _masterDbContext.SaveChangesAsync(HttpContext.RequestAborted);
        
        return new LanguageModel(language.Id, language.Code, language.DisplayName);
    }
    
    [HttpPatch("{code}")]
    [ProducesResponseType<LanguageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateLanguage(
        [FromRoute] string code,
        [FromBody] UpdateLanguageRequest request) 
    {
        if (!ModelState.IsValid)
            return BadRequest();
        
        var language = await _masterDbContext.Languages.SingleOrDefaultAsync(
            x => x.Code == code,
            HttpContext.RequestAborted);
        
        if (language is null)
            return NotFound();
        
        language.DisplayName = request.DisplayName;

        await _masterDbContext.SaveChangesAsync(HttpContext.RequestAborted);
        
        return Ok(new LanguageModel(language.Id, language.Code, language.DisplayName)); 
    }
}
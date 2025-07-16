using LocalizationService.Management.Api.Application;
using LocalizationService.Management.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Management.Api.Api.Controllers;


[ApiController]
[Route("api/version")]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
public class VersionController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<PublishSuccess>(StatusCodes.Status200OK)]
    [ProducesResponseType<PublishFailure>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PublishVersion([FromServices] IPublishChangesHandler publisher)
    {
        var result = await publisher.PublishTranslationsAsync(HttpContext.RequestAborted);

        return result.Match<IActionResult>(
            success => Ok(success),
            failure => UnprocessableEntity(failure));
    }
}
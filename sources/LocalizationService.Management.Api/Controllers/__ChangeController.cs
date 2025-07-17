// using LocalizationService.Domain.Changes;
// using LocalizationService.Management.Application.Translations.Changes;
// using LocalizationService.Management.Contracts;
// using LocalizationService.Management.Contracts.Changes;
// using Microsoft.AspNetCore.Mvc;
//
// namespace LocalizationService.Management.Api.Controllers;
//
//
// [ApiController]
// [Route("api/changes")]
// [Produces("application/json")]
// [Consumes("application/json")]
// [ProducesResponseType<ApiError>(StatusCodes.Status500InternalServerError)]
// public class ChangeController : ControllerBase
// {
//     [HttpGet]
//     [ProducesResponseType<ChangeListResponse>(StatusCodes.Status200OK)]
//     public async Task<ActionResult<ChangeListResponse>> GetChanges(
//         [FromServices] IChangeRepository repository,
//         [FromQuery] List<string> keys,
//         [FromQuery] int? skip = null,
//         [FromQuery] int? take = null)
//     {
//         var changes = await repository.GetChangeListAsync(keys, skip, take, HttpContext.RequestAborted);
//         var model = changes.Select(x => new ChangeModel(x.Id, x.Key, x.CurrentTranslations, x.CurrentTranslations));
//
//         return new ChangeListResponse(model.ToList());
//     }
//     
//     [HttpDelete("{id:long}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult> RevertChange(
//         [FromServices] IChangeRepository repository,
//         [FromServices] IChangeRevertService revertService,
//         [FromRoute] long id)
//     {
//         var change = await repository.GetChangeAsync(id, HttpContext.RequestAborted);
//
//         if (change is null)
//             return NotFound();
//
//         await revertService.RevertChangeAsync(change, HttpContext.RequestAborted);
//         
//         await repository.RemoveChangeAsync(change.Id, HttpContext.RequestAborted);
//
//         return NoContent();
//     }
//     
//     [HttpDelete]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     public async Task<ActionResult> RevertAllChanges(
//         [FromServices] IChangeRepository repository,
//         [FromServices] IChangeRevertService revertService,
//         [FromRoute] long id)
//     {
//         var changes = await repository.GetChangeListAsync([], cancellationToken: HttpContext.RequestAborted);
//
//         foreach (var change in changes)
//         {
//             await revertService.RevertChangeAsync(change, HttpContext.RequestAborted);
//         }
//
//         await repository.RemoveAllAsync(HttpContext.RequestAborted);
//
//         return NoContent();
//     }
// }
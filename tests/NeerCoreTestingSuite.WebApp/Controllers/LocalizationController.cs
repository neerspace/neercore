// using System.Net.Mime;
// using Microsoft.AspNetCore.JsonPatch;
// using Microsoft.AspNetCore.Mvc;
// using NeerCore.Api;
// using NeerCore.Localization;
//
// namespace NeerCoreTestingSuite.WebApp.Controllers;
//
// public record LocaModel
// {
//     /// <example>5</example>
//     public int Index { get; init; }
//
//     /// <example>LoremIpsum</example>
//     public LocalizedString Text { get; init; } = default!;
//
//     public required string Plain { get; set; }
//     public string? PlainNullableSt { get; set; }
// };
//
// [ApiController]
// [Route(DefaultRoutes.VersionedRoute)]
// [Consumes(MediaTypeNames.Application.Json)]
// public abstract class BaseLApi : ControllerBase { }
//
// public class LocalizationController : BaseLApi
// {
//     [HttpPatch("from-patch")]
//     public IActionResult PutPlain(JsonPatchDocument<LocaModel> document)
//     {
//         var model = new LocaModel { Plain = "test" };
//         document.ApplyTo(model);
//         return Ok(model);
//     }
//
//     [HttpPut("from-body/{id:int}")]
//     public async Task<IActionResult> PutFromBody([FromRoute] int id, [FromBody] LocaModel model) =>
//         Ok(model with { Index = id });
// }
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Api;
using NeerCore.Localization;

namespace NeerCoreTestingSuite.WebApp.Controllers;

public record LocaModel
{
    /// <example>5</example>
    public int Index { get; init; }

    /// <example>LoremIpsum</example>
    public LocalizedString Text { get; init; } = default!;
};

[ApiController]
[Route(DefaultRoutes.VersionedRoute)]
[Consumes(MediaTypeNames.Application.Json)]
public abstract class BaseLApi : ControllerBase { }

[Authorize]
public class LocalizationController : BaseLApi
{
    [HttpPost("from-nothing")]
    public IActionResult PutPlain(LocaModel model) => Ok(model);

    [AllowAnonymous]
    [HttpPut("from-body/{id:int}")]
    public async Task<IActionResult> PutFromBody([FromRoute] int id, [FromBody] LocaModel model) =>
        Ok(model with { Index = id });
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NeerCore.Api.Controllers;

/// <summary>Base API controller with NLog logger.</summary>
[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
    private ILogger? _logger;

    /// <summary>
    ///   Current controller logger instance.
    /// </summary>
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
}
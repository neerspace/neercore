using Microsoft.AspNetCore.Mvc;
using NLog;

namespace NeerCore.Api;

/// <summary>
/// Base API controller
/// </summary>
[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
	private ILogger? _logger;

	protected ILogger Logger => _logger ??= LogManager.GetLogger(GetType().Name);
}
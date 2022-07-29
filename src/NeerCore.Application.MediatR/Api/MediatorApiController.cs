using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace NeerCore.Api.Controllers;

/// <summary>
///   Base API controller with Mediator, with kebab-case routes and logger.
/// </summary>
[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
public abstract class MediatorController : ControllerBase
{
    private IMediator? _mediator;
    private ILogger? _logger;

    /// <summary>
    ///   Mediator instance.
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();


    /// <summary>
    ///   Current controller logger instance.
    /// </summary>
    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(GetType());
}
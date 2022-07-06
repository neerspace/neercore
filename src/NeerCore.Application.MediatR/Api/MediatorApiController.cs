using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Api.Controllers;

/// <summary>Base API controller with MediatR.</summary>
[ApiController]
[Route("/v{version:apiVersion}/[controller]")]
public abstract class MediatorController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
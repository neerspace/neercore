using Microsoft.AspNetCore.Mvc;

namespace NeerCore.Api.Controllers;

/// <summary>
///   Base API controller with with kebab-case routes, logger and route localization.
/// </summary>
[Route("/v{version:apiVersion}/{language:alpha:length(2)=en}/[controller]")]
public abstract class LocalizedApiController : ApiController { }
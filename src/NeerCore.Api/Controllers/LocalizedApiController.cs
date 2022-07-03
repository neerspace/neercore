using Microsoft.AspNetCore.Mvc;

namespace NeerCore.Api.Controllers;

// TODO: Review docs

/// <summary>Base API controller with logger and route localization.</summary>
[Route("/v{version:apiVersion}/{language:alpha:length(2)=en}/[controller]")]
public abstract class LocalizedApiController : ApiController { }
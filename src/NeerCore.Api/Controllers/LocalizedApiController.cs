using Microsoft.AspNetCore.Mvc;

namespace NeerCore.Api;

// TODO: Review docs

/// <summary>Base API controller with logger and route localization.</summary>
[Route("/v{version:apiVersion}/{languageCode:alpha(2)=en}/[controller]")]
public abstract class LocalizedApiController : ApiController { }
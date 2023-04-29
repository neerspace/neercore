using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Api;

namespace NeerCoreTestingSuite.WebApp.Controllers;

[ApiVersion("2.0")]
[AllowAnonymous]
[ApiController]
[Route(DefaultRoutes.VersionedRoute)]
// [Consumes(MediaTypeNames.Application.Json)]
public abstract class ApiController : ControllerBase { }
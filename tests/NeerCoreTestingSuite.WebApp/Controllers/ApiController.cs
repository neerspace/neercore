using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Api;

namespace NeerCoreTestingSuite.WebApp.Controllers;

[AllowAnonymous]
[ApiController]
[Route(DefaultRoutes.VersionedRoute)]
[Consumes(MediaTypeNames.Application.Json)]
public class ApiController : ControllerBase { }
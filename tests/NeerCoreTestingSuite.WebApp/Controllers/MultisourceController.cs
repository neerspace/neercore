using Microsoft.AspNetCore.Mvc;

namespace NeerCoreTestingSuite.WebApp.Controllers;

public record BeModel
{
    /// <example>776</example>
    [FromRoute(Name = "IDX")] public string Id { get; set; }

    /// <example>250</example>
    [FromRoute] public int Size { get; set; }

    /// <example>VlaDick</example>
    public string Name { get; set; }

    /// <example>VlaDick@theLongest.com</example>
    public required string Email { get; set; }
}

public class MultisourceController : ApiController
{
    [HttpPost("PostTest/{IDX:alpha}/{size:int}")]
    public BeModel Post([FromBody] BeModel model)
    {
        return model;
    }
}
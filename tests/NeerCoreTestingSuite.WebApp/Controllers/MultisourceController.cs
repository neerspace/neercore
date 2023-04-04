using Microsoft.AspNetCore.Mvc;
using NeerCore.Exceptions;

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

    [HttpGet("test-error-1")]
    public IActionResult Err1()
    {
        throw new ValidationFailedException("Validation err");
    }


    [HttpGet("test-error-2")]
    public IActionResult Err2()
    {
        throw new Exception("Server err");
    }


    [HttpGet("SimpleGet/{id}")]
    public IActionResult GetSmth([FromRoute] int id = 10, string other = "hi")
    {
        return Ok(new
        {
            Id = id,
            Other = other,
            Timestamp = DateTimeOffset.Now,
        });
    }
}
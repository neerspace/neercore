using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeerCore.Api.Controllers;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;

namespace NeerCoreTestingSuite.WebApp.Controllers;

public class LinqExceptionsController : ApiController
{
    private readonly IDatabaseContext _database;
    public LinqExceptionsController(IDatabaseContext database) => _database = database;

    [HttpGet("first")]
    public async Task<Tea> FirstAsync(string name)
    {
        return await _database.Set<Data.Entities.Tea>()
            .Where(t => t.Name == name)
            .ProjectToType<Tea>()
            .FirstAsync();
    }

    [HttpGet("single")]
    public async Task<Tea> SingleAsync(string name)
    {
        return await _database.Set<Data.Entities.Tea>()
            .Where(t => t.Name == name)
            .ProjectToType<Tea>()
            .SingleAsync();
    }
}
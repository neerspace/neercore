using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Api.Controllers;
using NeerCore.Api.Extensions;
using NeerCore.Mapping.Extensions;
using NeerCoreTestingSuite.WebApp.Dto.Teas;
using NeerCoreTestingSuite.WebApp.Services;

namespace NeerCoreTestingSuite.WebApp.Controllers;

public class TeasController : LocalizedApiController
{
    private readonly TeasService _service;
    public TeasController(TeasService service) => _service = service;


    [HttpGet("{id:guid}")]
    public async Task<Tea> Get([FromRoute] Guid id)
    {
        var entity = await _service.GetByIdAsync(id);
        return entity.Adapt<Tea>();
    }

    [HttpGet]
    public async Task<IEnumerable<Tea>> Filter(string filters, string sorts = "id", int page = 1, int pageSize = 10)
    {
        var entities = await _service.FilterAsync(filters, sorts, page, pageSize);
        Response.SetNavigationHeaders(await _service.CountAsync(), page, pageSize);
        return entities.AdaptAll<Tea>();
    }

    [HttpPost]
    public async Task<ActionResult<Tea>> Post([FromBody] TeaCreate dto)
    {
        var entity = dto.Adapt<Data.Entities.Tea>();
        await _service.CreateAsync(entity);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity.Adapt<Tea>());
    }

    [HttpPut("{id:guid}")]
    public async Task<NoContentResult> Put([FromRoute] Guid id, [FromBody] TeaUpdate dto)
    {
        var entity = (dto with { Id = id }).Adapt<Data.Entities.Tea>();
        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<NoContentResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument<TeaUpdate> patch)
    {
        var entity = await _service.GetByIdAsync(id);
        var dto = entity.Adapt<TeaUpdate>();
        patch.ApplyTo(dto);
        entity = dto.Adapt<Data.Entities.Tea>();
        await _service.UpdateAsync(entity);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<NoContentResult> Delete([FromRoute] Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
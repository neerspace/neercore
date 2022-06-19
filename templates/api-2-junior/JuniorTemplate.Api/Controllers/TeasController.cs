using JuniorTemplate.Application.Models.Teas;
using JuniorTemplate.Application.Services;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NeerCore.Api;
using NeerCore.Api.Extensions;
using NeerCore.Mapping.Extensions;

namespace JuniorTemplate.Api.Controllers;

public class TeasController : ApiController
{
	private readonly ITeasService _service;
	public TeasController(ITeasService service) => _service = service;


	[HttpGet("{id:guid}")]
	public async Task<TeaModel> Get([FromRoute] Guid id)
	{
		var dto = await _service.GetByIdAsync(id);
		return dto.Adapt<TeaModel>();
	}

	[HttpGet]
	public async Task<IEnumerable<TeaModel>> Filter(string filters, string sorts = "id", int page = 1, int pageSize = 10)
	{
		var dto = await _service.FilterAsync(filters, sorts, page, pageSize);
		Response.SetNavigationHeaders(await _service.CountAsync(), page, pageSize);
		return dto.AdaptAll<TeaModel>();
	}

	[HttpPost]
	public async Task<ActionResult<TeaModel>> Post([FromBody] TeaCreateModel dto)
	{
		var model = dto.Adapt<Data.Entities.Tea>();
		await _service.CreateAsync(model);
		return CreatedAtAction(nameof(Get), new { id = model.Id }, model.Adapt<TeaModel>());
	}

	[HttpPut("{id:guid}")]
	public async Task<NoContentResult> Put([FromRoute] Guid id, [FromBody] TeaUpdateModel dto)
	{
		var model = (dto with { Id = id }).Adapt<Data.Entities.Tea>();
		await _service.UpdateAsync(model);
		return NoContent();
	}

	[HttpPatch("{id:guid}")]
	public async Task<NoContentResult> Patch([FromRoute] Guid id, [FromBody] JsonPatchDocument patch)
	{
		var model = await _service.GetByIdAsync(id);
		patch.ApplyTo(model);
		await _service.UpdateAsync(model.Adapt<Data.Entities.Tea>());
		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<NoContentResult> Delete([FromRoute] Guid id)
	{
		await _service.DeleteAsync(id);
		return NoContent();
	}
}
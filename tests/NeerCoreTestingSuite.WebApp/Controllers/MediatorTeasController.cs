// using Microsoft.AspNetCore.Mvc;
// using NeerCore.Api.Controllers;
// using NeerCoreTestingSuite.WebApp.Dto.Teas;
// using NeerCoreTestingSuite.WebApp.Features.Teas;
//
// namespace NeerCoreTestingSuite.WebApp.Controllers;
//
// public class MediatorTeasController : MediatorController
// {
// 	[HttpGet("{id:guid}")]
// 	public async Task<Tea> Get([FromRoute] Guid id)
// 	{
// 		return await Mediator.Send(new GetTeaQuery(id));
// 	}
//
// 	[HttpPost]
// 	public async Task<ActionResult<Tea>> Post([FromBody] CreateTeaCommand command)
// 	{
// 		Tea tea = await Mediator.Send(command);
// 		return CreatedAtAction(nameof(TeasController.Get), new { id = tea.Id }, tea);
// 	}
// }
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NeerCore.Api.Extensions;
using NeerCore.Exceptions;
using NLog;

namespace NeerCore.Api.Defaults.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
	protected readonly ILogger Logger;

	public ExceptionHandlerMiddleware()
	{
		Logger = LogManager.GetCurrentClassLogger();
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (ValidationException e)
		{
			await context.Response.WriteJsonAsync(HttpStatusCode.BadRequest, e.CreateFluentValidationError());
		}
		catch (HttpException e)
		{
			if ((int) e.StatusCode >= 500)
			{
				Logger.Error(e, "Internal Server Error");
				await context.Response.Write500ErrorAsync(e);
			}
			else
				await context.Response.WriteJsonAsync(e.StatusCode, e.CreateError());
		}
		catch (Exception e)
		{
			Logger.Error(e, "Unhandled Server Error");
			await context.Response.Write500ErrorAsync(e);
		}
	}
}
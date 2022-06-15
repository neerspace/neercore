using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NeerCore.Exceptions;

namespace NeerCore.Api.Defaults.Middleware;

public class ExceptionHandlerMiddleware : HttpExceptionHandler, IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (ValidationException e)
		{
			await WriteJsonResponseAsync(context, HttpStatusCode.BadRequest, CreateFluentValidationError(e));
		}
		catch (HttpException e)
		{
			if ((int) e.StatusCode >= 500)
				await Write500StatusCodeResponseAsync(context, e);
			else
				await WriteJsonResponseAsync(context, e.StatusCode, CreateError(e));
		}
		catch (Exception e)
		{
			await Write500StatusCodeResponseAsync(context, e);
		}
	}
}
using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NeerCore.Exceptions;
using NLog;

namespace NeerCore.Api;

public abstract class HttpExceptionHandler
{
	protected readonly ILogger Logger;

	public HttpExceptionHandler()
	{
		Logger = LogManager.GetCurrentClassLogger();
	}


	protected static async Task WriteJsonResponseAsync<T>(HttpContext context, HttpStatusCode statusCode, T response)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int) statusCode;
		await context.Response.WriteAsync(JsonSerializer.Serialize(response, GlobalJsonConventions.CamelCase));
	}

	protected async Task Write500StatusCodeResponseAsync(HttpContext context, Exception exception)
	{
		Logger.Error(exception, "Internal Server Error");

		var error = CreateError(new InternalServerException(exception.Message));
		await WriteJsonResponseAsync(context, HttpStatusCode.InternalServerError, error);
	}

	protected async Task WriteExtended500StatusCodeResponseAsync(HttpContext context, Exception exception)
	{
		Logger.Error(exception, "Internal Server Error");

		context.Response.ContentType = "text/plain";
		context.Response.StatusCode = StatusCodes.Status500InternalServerError;
		await context.Response.WriteAsync($"===== SERVER ERROR =====\n{exception}\n===== ===== ===== =====");
	}

	protected static Error CreateError(HttpException e) => new(
		status: (int) e.StatusCode,
		type: e.ErrorType,
		message: e.Message,
		errors: e.Details?.Select(ed => new Error.Details(ed.Field, ed.Message)).ToArray()
	);

	protected static Error CreateFluentValidationError(ValidationException e) => new(
		status: 400,
		type: "ValidationFailed",
		message: "Invalid model received.",
		errors: e.Errors.Select(ve => new Error.Details(ve.PropertyName, ve.ErrorMessage)).ToArray()
	);
}
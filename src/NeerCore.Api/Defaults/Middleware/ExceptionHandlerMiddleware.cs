using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NeerCore.Api.Extensions;
using NeerCore.Exceptions;

namespace NeerCore.Api.Defaults.Middleware;

/// <summary>
///   Default NeerCore exception handling middleware to handle
///   a <see cref="HttpException"/> with custom formatted error messages
///   and default 500 exception with fine view otherwise.
/// </summary>
public class ExceptionHandlerMiddleware : IMiddleware
{
    protected readonly ILogger Logger;

    public ExceptionHandlerMiddleware(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
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
            if ((int)e.StatusCode >= 500)
            {
                Logger.LogError(e, "Internal Server Error");
                await context.Response.Write500ErrorAsync(e);
            }
            else
                await context.Response.WriteJsonAsync(e.StatusCode, e.CreateError());
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Unhandled Server Error");
            await context.Response.Write500ErrorAsync(e);
        }
    }
}
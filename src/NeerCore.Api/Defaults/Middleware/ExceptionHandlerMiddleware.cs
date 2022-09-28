using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NeerCore.Api.Extensions;
using NeerCore.Api.Options;
using NeerCore.Exceptions;

namespace NeerCore.Api.Defaults.Middleware;

/// <summary>
///   Default NeerCore exception handling middleware to handle
///   a <see cref="HttpException"/> with custom formatted error messages
///   and default 500 exception with fine view otherwise.
/// </summary>
public sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly ExceptionHandlerOptions _options;

    public ExceptionHandlerMiddleware(ILoggerFactory loggerFactory, IOptions<ExceptionHandlerOptions> optionsAccessor)
    {
        _logger = loggerFactory.CreateLogger(GetType());
        _options = optionsAccessor.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException e)
        {
            if (_options.HandleFluentValidationExceptions)
            {
                await context.Response.WriteJsonAsync(HttpStatusCode.BadRequest, e.CreateFluentValidationError());
            }
            else
            {
                await ProcessCommonExceptionAsync(context, e);
            }
        }
        catch (HttpException e)
        {
            if (_options.HandleHttpExceptions)
            {
                if ((int)e.StatusCode >= 500)
                {
                    _logger.LogError(e, "Internal Server Error");
                    await context.Response.Write500ErrorAsync(e, _options.Extended500ExceptionMessage);
                }
                else
                {
                    await context.Response.WriteJsonAsync(e.StatusCode, e.CreateError());
                }
            }
            else
            {
                await ProcessCommonExceptionAsync(context, e);
            }
        }
        catch (Exception e)
        {
            await ProcessCommonExceptionAsync(context, e);
        }
    }

    private async Task ProcessCommonExceptionAsync(HttpContext context, Exception e)
    {
        _logger.LogError(e, "Unhandled Server Error");
        await context.Response.Write500ErrorAsync(e, _options.Extended500ExceptionMessage);
    }
}
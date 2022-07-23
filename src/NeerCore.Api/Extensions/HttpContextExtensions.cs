using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NeerCore.Exceptions;
using NeerCore.Json;

namespace NeerCore.Api.Extensions;

public static class HttpContextExtensions
{
    /// <summary>
    ///   Gets IP address from request headers.
    /// </summary>
    public static IPAddress GetIPAddress(this HttpContext httpContext)
    {
        var headerValue = httpContext.Request.Headers["X-Forwarded-For"];

        if (headerValue == StringValues.Empty)
        {
            // this will always have a value (running locally in development won't have the header)
            return httpContext.Request.HttpContext.Connection.RemoteIpAddress!;
        }

        // when running behind a load balancer you can expect this header
        var header = headerValue.ToString();

        // in case the IP contains a port, remove ':' and everything after
        int sepIndex = header.IndexOf(':');
        header = sepIndex == -1 ? header : header.Remove(sepIndex);
        return IPAddress.Parse(header);
    }

    /// <summary>
    ///   Gets user agent from request headers.
    /// </summary>
    public static string GetUserAgent(this HttpContext httpContext)
    {
        return httpContext.Request.Headers["User-Agent"].ToString();
    }

    public static async Task WriteJsonAsync<T>(this HttpResponse context, HttpStatusCode statusCode, T response)
    {
        context.ContentType = "application/json";
        context.StatusCode = (int)statusCode;
        await context.WriteAsync(JsonSerializer.Serialize(response, JsonConventions.CamelCase));
    }

    public static async Task Write500ErrorAsync(this HttpResponse context, Exception exception)
    {
        var error = new InternalServerException(exception.Message).CreateError();
        await context.WriteJsonAsync(HttpStatusCode.InternalServerError, error);
    }

    public static async Task WriteExtended500ErrorAsync(this HttpResponse context, Exception exception)
    {
        context.ContentType = "text/plain";
        context.StatusCode = StatusCodes.Status500InternalServerError;
        await context.WriteAsync($"===== SERVER ERROR =====\n{exception}\n===== ===== ===== =====");
    }
}
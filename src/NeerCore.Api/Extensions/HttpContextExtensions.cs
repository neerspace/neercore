using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NeerCore.Exceptions;
using NeerCore.Json;

namespace NeerCore.Api.Extensions;

public static class HttpContextExtensions
{
    public static async Task WriteJsonAsync<T>(this HttpResponse context, HttpStatusCode statusCode, T response)
    {
        context.ContentType = "application/json";
        context.StatusCode = (int) statusCode;
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
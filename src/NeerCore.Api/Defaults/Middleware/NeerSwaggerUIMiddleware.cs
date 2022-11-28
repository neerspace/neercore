using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NeerCore.Api.Defaults.Middleware;

public class NeerSwaggerUIMiddleware
{
    private const string EmbeddedFileNamespace = "Swashbuckle.AspNetCore.SwaggerUI.node_modules.swagger_ui_dist";

    private readonly SwaggerUIOptions _options;
    private readonly StaticFileMiddleware _staticFileMiddleware;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public NeerSwaggerUIMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILoggerFactory loggerFactory, SwaggerUIOptions? options)
    {
        _options = options ?? new SwaggerUIOptions();

        _staticFileMiddleware = CreateStaticFileMiddleware(next, environment, loggerFactory, options!);

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
    }

    public async Task Invoke(HttpContext httpContext)
    {
        string httpMethod = httpContext.Request.Method;
        string path = httpContext.Request.Path.Value!;

        switch (httpMethod)
        {
            case "GET" when Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase):
                RespondWithRedirect(httpContext.Response, _options.RoutePrefix);
                return;

            case "GET" when Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase):
                await RespondWithIndexHtml(httpContext.Response);
                return;

            default:
                await _staticFileMiddleware.Invoke(httpContext);
                return;
        }
    }

    private StaticFileMiddleware CreateStaticFileMiddleware(
        RequestDelegate next,
        IWebHostEnvironment hostingEnv,
        ILoggerFactory loggerFactory,
        SwaggerUIOptions options)
    {
        var staticFileOptions = new StaticFileOptions
        {
            RequestPath = string.IsNullOrEmpty(options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}",
            FileProvider = new EmbeddedFileProvider(typeof(SwaggerUIMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
        };

        return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
    }

    private void RespondWithRedirect(HttpResponse response, string location)
    {
        response.StatusCode = 301;
        response.Headers["Location"] = location;
    }

    private async Task RespondWithIndexHtml(HttpResponse response)
    {
        response.StatusCode = 200;
        response.ContentType = "text/html;charset=utf-8";

        await using var stream = _options.IndexStream();
        // Inject arguments before writing to response
        var htmlBuilder = new StringBuilder(await new StreamReader(stream).ReadToEndAsync());
        foreach (var entry in GetIndexArguments())
        {
            htmlBuilder.Replace(entry.Key, entry.Value);
        }

        await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
    }

    private IDictionary<string, string> GetIndexArguments()
    {
        return new Dictionary<string, string>()
        {
            { "%(DocumentTitle)", _options.DocumentTitle },
            { "%(HeadContent)", _options.HeadContent },
            { "%(ConfigObject)", JsonSerializer.Serialize(_options.ConfigObject, _jsonSerializerOptions) },
            { "%(OAuthConfigObject)", JsonSerializer.Serialize(_options.OAuthConfigObject, _jsonSerializerOptions) },
            { "%(Interceptors)", JsonSerializer.Serialize(_options.Interceptors) },
        };
    }
}
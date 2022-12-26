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
using NeerCore.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NeerCore.Api.Middleware;

public class NeerSwaggerUIMiddleware
{
    private const string SwaggerEmbeddedFileNamespace = "Swashbuckle.AspNetCore.SwaggerUI.node_modules.swagger_ui_dist";
    private const string NeerEmbeddedFileNamespace = "NeerCore.Api.wwwroot";
    private const string EmbeddedPluginsScriptPath = NeerEmbeddedFileNamespace + ".swagger-plugins.js";

    private readonly SwaggerUIOptions _options;
    private readonly SwaggerConfigurationOptions _swaggerConfiguration;
    private readonly StaticFileMiddleware _swaggerStaticFileMiddleware;
    private readonly StaticFileMiddleware _neerStaticFileMiddleware;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly RequestDelegate _next;

    public NeerSwaggerUIMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILoggerFactory loggerFactory, SwaggerUIOptions? options, IOptions<SwaggerConfigurationOptions> swaggerOptionsAccessor)
    {
        _next = next;
        _swaggerConfiguration = swaggerOptionsAccessor.Value;
        _options = options ?? new SwaggerUIOptions();

        _swaggerStaticFileMiddleware = CreateSwaggerStaticFileMiddleware(next, environment, loggerFactory);
        _neerStaticFileMiddleware = CreateNeerStaticFileMiddleware(next, environment, loggerFactory);

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

        if (httpMethod == "GET")
        {
            if (Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase))
            {
                RespondWithRedirect(httpContext.Response, _options.RoutePrefix);
            }
            else if (Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
            }
            else
            {
                if (_swaggerConfiguration.ExtendedDocs)
                {
                    await _neerStaticFileMiddleware.Invoke(httpContext);
                }

                if (!httpContext.Response.HasStarted)
                {
                    await _swaggerStaticFileMiddleware.Invoke(httpContext);
                }
            }
        }
        else
        {
            await _next(httpContext);
        }
    }


    private StaticFileMiddleware CreateSwaggerStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
    {
        var staticFileOptions = new StaticFileOptions
        {
            RequestPath = string.IsNullOrEmpty(_options.RoutePrefix) ? string.Empty : $"/{_options.RoutePrefix}",
            FileProvider = new EmbeddedFileProvider(typeof(SwaggerUIMiddleware).Assembly, SwaggerEmbeddedFileNamespace),
        };

        return new StaticFileMiddleware(next, environment, Options.Create(staticFileOptions), loggerFactory);
    }

    private StaticFileMiddleware CreateNeerStaticFileMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
    {
        var staticFileOptions = new StaticFileOptions
        {
            RequestPath = "/neercore",
            FileProvider = new EmbeddedFileProvider(typeof(NeerSwaggerUIMiddleware).GetTypeInfo().Assembly, NeerEmbeddedFileNamespace),
        };

        var resx = typeof(NeerSwaggerUIMiddleware).Assembly.GetManifestResourceNames();

        return new StaticFileMiddleware(next, environment, Options.Create(staticFileOptions), loggerFactory);
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

        htmlBuilder.Replace("href=\"./", $"href=\"/{_options.RoutePrefix}/");
        htmlBuilder.Replace("src=\"./", $"src=\"/{_options.RoutePrefix}/");

        await using var pluginsScript = GetType().Assembly.GetManifestResourceStream(EmbeddedPluginsScriptPath);
        if (pluginsScript is not null)
        {
            using var reader = new StreamReader(pluginsScript);
            const string searchPhrase = "const ui = SwaggerUIBundle(configObject);";
            string javascriptText = await reader.ReadToEndAsync();
            htmlBuilder.Replace(searchPhrase, javascriptText + "\n" + searchPhrase);
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
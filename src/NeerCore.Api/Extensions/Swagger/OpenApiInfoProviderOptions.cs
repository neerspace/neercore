using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace NeerCore.Api.Extensions.Swagger;

internal class OpenApiInfoProviderOptions
{
    public Func<ApiVersionDescription, OpenApiInfo>? ConfigureDelegate { get; set; }
}
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    [Obsolete("Use direct call of builder.Services.AddNeerApiServices() instead of this.")]
    public static IMvcBuilder AddNeerApi(this WebApplicationBuilder builder) =>
        builder.AddNeerApi(Assembly.GetCallingAssembly());

    [Obsolete("Use direct call of builder.Services.AddNeerApiServices(string[]) instead of this.")]
    public static IMvcBuilder AddNeerApi(this WebApplicationBuilder builder, params string[] assemblyNames) =>
        builder.AddNeerApi(assemblyNames.Select(Assembly.Load).ToArray());

    [Obsolete("Use direct call of builder.Services.AddNeerApiServices(Assembly[]) instead of this.")]
    public static IMvcBuilder AddNeerApi(this WebApplicationBuilder builder, params Assembly[] assemblies)
    {
        builder.Services.AddNeerApiServices(assemblies);

        return builder.Services.AddControllers(KebabCaseNamingConvention.Use)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }
}
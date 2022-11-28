using AspNetCore.Hashids.Options;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Typeids.Api.Hashids.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTypeidsWithHashids(this IServiceCollection services, Action<HashidsOptions>? setup = null)
    {
        setup ??= _ => { };
        services.AddHashids(setup);
        services.AddScoped<ITypeidsProcessor, TypeidsHashidsProcessor>();
        return services;
    }
}
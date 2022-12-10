using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Typeids.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTypeidsWithHashids(this IServiceCollection services)
    {
        services.AddScoped<ITypeidsProcessor, TypeidsProcessor>();
        return services;
    }
}
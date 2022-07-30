using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Design;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Adds a <see cref="DbContext"/> to DI container as <see cref="IDatabaseContext"/> abstraction.
    /// </summary>
    public static void AddDatabase<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction)
        where TContext : DbContext, IDatabaseContext
    {
        services.AddDbContext<TContext>(optionsAction);
        services.AddScoped<IDatabaseContext, TContext>();
    }

    /// <summary>
    ///   Adds a <see cref="DbContext"/> to DI container as <see cref="IDatabaseContext"/> abstraction
    ///   and use configuration from <typeparamref name="TContextFactory"/>.
    /// </summary>
    public static void AddDatabase<TContext, TContextFactory>(this IServiceCollection services)
        where TContextFactory : DbContextFactoryBase<TContext>, new()
        where TContext : DbContext, IDatabaseContext
    {
        var contextFactory = new TContextFactory();
        services.AddDbContext<TContext>(contextFactory.ConfigureContextOptions);

        services.AddScoped<IDatabaseContext, TContext>();
    }
}
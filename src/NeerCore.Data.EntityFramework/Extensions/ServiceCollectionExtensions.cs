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
    /// <typeparam name="TContext">The type of context to be registered.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="optionsAction">
    ///     <para>
    ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
    ///         alternative to performing configuration of the context by overriding the
    ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
    ///     </para>
    ///     <para>
    ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
    ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
    ///         in addition to configuration performed here.
    ///     </para>
    ///     <para>
    ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
    ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
    ///     </para>
    /// </param>
    /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container.</param>
    /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static void AddDatabase<TContext>(this IServiceCollection services,
        Action<DbContextOptionsBuilder>? optionsAction = null,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TContext : DbContext, IDatabaseContext
    {
        services.AddDbContext<TContext>(optionsAction, contextLifetime, optionsLifetime);
        services.Add(new ServiceDescriptor(typeof(IDatabaseContext), typeof(TContext), contextLifetime));
    }

    /// <summary>
    ///   Adds a <see cref="DbContext"/> to DI container as <see cref="IDatabaseContext"/> abstraction
    ///   and use configuration from <typeparamref name="TContextFactory"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of context to be registered.</typeparam>
    /// <typeparam name="TContextFactory">The type of the context factory for <typeparamref name="TContext"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="contextLifetime">The lifetime with which to register the DbContext service in the container.</param>
    /// <param name="optionsLifetime">The lifetime with which to register the DbContextOptions service in the container.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static void AddDatabase<TContext, TContextFactory>(this IServiceCollection services,
        ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
        ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        where TContextFactory : DbContextFactoryBase<TContext>, new()
        where TContext : DbContext, IDatabaseContext
    {
        var contextFactory = new TContextFactory();
        services.AddDbContext<TContext>(contextFactory.ConfigureContextOptions, contextLifetime, optionsLifetime);

        services.Add(new ServiceDescriptor(typeof(IDatabaseContext), typeof(TContext), contextLifetime));
    }
}
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    ///   Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}"/>
    ///   instances that are defined in provided <b>calling assembly</b>.
    /// </summary>
    /// <param name="builder">Database model builder.</param>
    [Obsolete("Use 'ApplyAllConfigurations' instead of this.")]
    public static void ApplyConfigurations(this ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }

    public static void ApplyAllConfigurations(this ModelBuilder builder)
    {
        builder.ApplyConfigurations();
    }

    /// <param name="assemblyName"><see cref="Assembly"/> name where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyDataSeeders"/>
    public static void ApplyDataSeedersFromAssembly(this ModelBuilder builder, string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    /// <param name="assembly"><see cref="Assembly"/> where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyDataSeeders"/>
    public static void ApplyDataSeedersFromAssembly(this ModelBuilder builder, Assembly assembly)
    {
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    /// <summary>
    ///   Applies all <see cref="IEntityDataSeeder{TEntity}"/> and <see cref="IDataSeeder"/>
    ///   seeding data from provided assembly or from the calling assembly by default.
    /// </summary>
    /// <param name="builder">Database model builder.</param>
    [Obsolete("Use 'ApplyAllDataSeeders' instead of this.")]
    public static void ApplyDataSeeders(this ModelBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    public static void ApplyAllDataSeeders(this ModelBuilder builder)
    {
        builder.ApplyDataSeeders();
    }

    private static void ApplyEntityDataSeeders(ModelBuilder builder, Assembly? assembly = null)
    {
        var interfaceType = typeof(IEntityDataSeeder<>);
        var seeders = AssemblyProvider.GetImplementationsOf(interfaceType);
        if (assembly is not null) seeders = seeders.Where(s => s.Assembly == assembly);
        foreach (Type seederType in seeders)
        {
            var entityType = seederType.GetInterface(interfaceType.Name)!.GetGenericArguments().First();
            var seederInstance = Activator.CreateInstance(seederType)!;
            var seedDataProperty = seederInstance.GetType().GetRuntimeProperty("Data")!.GetGetMethod();
            var data = seedDataProperty!.Invoke(seederInstance, null) as object[];
            builder.Entity(entityType).HasData(data!);
        }
    }

    private static void ApplyExtendedDataSeeders(ModelBuilder builder, Assembly? assembly = null)
    {
        var interfaceType = typeof(IDataSeeder);
        var seeders = AssemblyProvider.GetImplementationsOf(interfaceType);
        if (assembly is not null) seeders = seeders.Where(s => s.Assembly == assembly);
        foreach (Type seederType in seeders)
        {
            var seederInstance = Activator.CreateInstance(seederType)!;
            var seedMethod = seederInstance.GetType().GetMethod(nameof(IDataSeeder.Seed), new[] { builder.GetType() })!;
            seedMethod.Invoke(seederInstance, new object?[] { builder });
        }
    }
}
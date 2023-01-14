using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.DependencyInjection;
using NeerCore.Localization;

namespace NeerCore.Data.EntityFramework.Extensions;

// TODO: Add docs

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configure entities
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    public static ModelBuilder ConfigureEntities(this ModelBuilder builder, Action<DbDesignOptions>? configureOptions = null)
    {
        var options = new DbDesignOptions();
        configureOptions?.Invoke(options);
        options.DataAssemblies ??= new[] { Assembly.GetCallingAssembly() };

        // builder.AddAllEntities();
        builder.ApplyEntityIds(options);
        builder.ApplyEntityDating(options);

        foreach (Assembly? dataAssembly in options.DataAssemblies)
        {
            if (dataAssembly is null)
                continue;

            builder.AddLocalizedStrings(dataAssembly);
            if (options.ApplyEntityTypeConfigurations)
                builder.ApplyConfigurationsFromAssembly(dataAssembly);
            if (options.ApplyDataSeeders)
                builder.ApplyDataSeedersFromAssembly(dataAssembly);
        }

        return builder;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    public static ModelBuilder ApplyEntityIds(this ModelBuilder builder, DbDesignOptions options)
    {
        Type entityWithIdType = typeof(IEntity<>);

        foreach (Type entityType in AssemblyProvider.GetImplementationsOf(entityWithIdType))
        {
            // TODO: Temp decision
            Type entityIdType = entityType.GetInterfaces().First(it => it.Name.StartsWith("IEntity`")).GetGenericArguments().First();
            EntityTypeBuilder entityBuilder = builder.Entity(entityType);
            entityBuilder.HasKey("Id");
            PropertyBuilder idPropertyBuilder = entityBuilder.Property("Id");

            if (entityIdType == typeof(Guid))
            {
                if (options is { PreferSqlSideDefaultValues: true, EngineStrategy: DbEngineStrategy.SqlServer })
                    idPropertyBuilder.HasDefaultValueSql(options.SequentialGuids ? "NEWSEQUENTIALID()" : "NEWID()").ValueGeneratedOnAdd();
                else
                    idPropertyBuilder.HasDefaultValue(Guid.NewGuid()).ValueGeneratedOnAdd();
            }
            else if (entityIdType == typeof(string))
            {
                idPropertyBuilder.HasDefaultValue(Guid.NewGuid().ToString());
            }
        }

        return builder;
    }

    public static IEnumerable<EntityTypeBuilder> GetAllEntities(this ModelBuilder builder, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();
        foreach (Type entityType in AssemblyProvider.GetImplementationsFromAssembly<IEntity>(assembly))
        {
            yield return builder.Entity(entityType);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assembly"></param>
    public static ModelBuilder AddAllEntities(this ModelBuilder builder, Assembly? assembly = null)
    {
        _ = builder.GetAllEntities(assembly).Count();
        return builder;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assembly"></param>
    public static ModelBuilder AddLocalizedStrings(this ModelBuilder builder, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        var valueComparer = new ValueComparer<LocalizedString>(
            (ls1, ls2) => ls1.Equals(ls2),
            ls => ls.GetHashCode(),
            ls => new LocalizedString(ls));
        var localizedStringType = typeof(LocalizedString);

        foreach (Type entityType in AssemblyProvider.GetImplementationsFromAssembly<IEntity>(assembly))
        {
            var entityTypeBuilder = builder.Entity(entityType);
            var localizedStringProperties = entityType.GetProperties()
                .Where(p => p.PropertyType == localizedStringType);
            foreach (var property in localizedStringProperties)
            {
                entityTypeBuilder.Property(property.Name)
                    .Metadata.SetValueComparer(valueComparer);
            }
        }

        return builder;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    public static ModelBuilder ApplyEntityDating(this ModelBuilder builder, Action<DbDesignOptions>? configureOptions = null)
    {
        var options = new DbDesignOptions();
        configureOptions?.Invoke(options);
        return builder.ApplyEntityDating(options);
    }

    private static ModelBuilder ApplyEntityDating(this ModelBuilder builder, DbDesignOptions options)
    {
        bool utc = options.DateTimeKind is DateTimeKind.Utc;
        string defaultValueSql = options.EngineStrategy switch
        {
            DbEngineStrategy.SqlServer => utc ? "SYSUTCDATETIME()" : "SYSDATETIME()",
            DbEngineStrategy.Sqlite => utc ? "DATETIME('now')" : "DATETIME()",
            DbEngineStrategy.Postgres => utc ? "TIMEZONE('utc', NOW())" : "NOW()",
            _ => throw new ArgumentException($"{nameof(DbEngineStrategy)} of {options.EngineStrategy} is not currently supported.")
        };

        options.DataAssemblies ??= new[] { Assembly.GetCallingAssembly() };
        foreach (var assembly in options.DataAssemblies)
        {
            if (assembly is null)
                continue;

            foreach (Type creatableEntityType in AssemblyProvider.GetImplementationsFromAssembly<ICreatableEntity>(assembly))
            {
                builder.Entity(creatableEntityType)
                    .Property(nameof(ICreatableEntity.Created))
                    .HasDefaultValueSql(defaultValueSql)
                    .ValueGeneratedOnAdd();
            }

            foreach (Type updatableEntityType in AssemblyProvider.GetImplementationsFromAssembly<IUpdatableEntity>(assembly))
            {
                builder.Entity(updatableEntityType)
                    .Property(nameof(IUpdatableEntity.Updated))
                    .HasDefaultValueSql(defaultValueSql)
                    .ValueGeneratedOnUpdate();
            }
        }

        return builder;
    }

    /// <summary>
    ///   Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}"/>
    ///   instances that are defined in provided <b>calling assembly</b>.
    /// </summary>
    /// <param name="builder">Database model builder.</param>
    public static ModelBuilder ApplyAllConfigurations(this ModelBuilder builder)
    {
        return builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }

    /// <param name="assemblyName"><see cref="Assembly"/> name where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyAllDataSeeders"/>
    public static ModelBuilder ApplyDataSeedersFromAssembly(this ModelBuilder builder, string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        return builder
            .ApplyEntityDataSeeders(assembly)
            .ApplyExtendedDataSeeders(assembly);
    }

    /// <param name="assembly"><see cref="Assembly"/> where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyAllDataSeeders"/>
    public static ModelBuilder ApplyDataSeedersFromAssembly(this ModelBuilder builder, Assembly assembly)
    {
        return builder
            .ApplyEntityDataSeeders(assembly)
            .ApplyExtendedDataSeeders(assembly);
    }

    /// <summary>
    ///   Applies all <see cref="IEntityDataSeeder{TEntity}"/> and <see cref="IDataSeeder"/>
    ///   seeding data from provided assembly or from the calling assembly by default.
    /// </summary>
    /// <param name="builder">Database model builder.</param>
    public static ModelBuilder ApplyAllDataSeeders(this ModelBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
        return builder;
    }

    private static ModelBuilder ApplyEntityDataSeeders(this ModelBuilder builder, Assembly assembly)
    {
        var interfaceType = typeof(IEntityDataSeeder<>);
        var seeders = AssemblyProvider.GetImplementationsFromAssembly(interfaceType, assembly);
        foreach (Type seederType in seeders)
        {
            var entityType = seederType.GetInterface(interfaceType.Name)!.GetGenericArguments().First();
            var seederInstance = Activator.CreateInstance(seederType)!;
            var seedDataProperty = seederInstance.GetType().GetRuntimeProperty("Data")!.GetGetMethod();
            var data = seedDataProperty!.Invoke(seederInstance, null) as object[];
            builder.Entity(entityType).HasData(data!);
        }

        return builder;
    }

    private static ModelBuilder ApplyExtendedDataSeeders(this ModelBuilder builder, Assembly assembly)
    {
        var interfaceType = typeof(IDataSeeder);
        var seeders = AssemblyProvider.GetImplementationsFromAssembly(interfaceType, assembly);
        foreach (Type seederType in seeders)
        {
            var seederInstance = Activator.CreateInstance(seederType)!;
            var seedMethod = seederInstance.GetType().GetMethod(nameof(IDataSeeder.Seed), new[] { builder.GetType() })!;
            seedMethod.Invoke(seederInstance, new object?[] { builder });
        }

        return builder;
    }
}
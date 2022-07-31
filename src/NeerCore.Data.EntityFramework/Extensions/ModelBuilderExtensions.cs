using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeerCore.Data.Abstractions;
using NeerCore.Data.EntityFramework.Abstractions;
using NeerCore.Data.EntityFramework.Design;
using NeerCore.DependencyInjection;

namespace NeerCore.Data.EntityFramework.Extensions;

// TODO: Add docs

public static class ModelBuilderExtensions
{
    /// <summary>
    ///   
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    public static void ConfigureEntities(this ModelBuilder builder, Action<DbDesignOptions>? configureOptions = null)
    {
        var options = new DbDesignOptions();
        configureOptions?.Invoke(options);
        options.DataAssemblies ??= new[] { Assembly.GetCallingAssembly() };

        builder.RegisterAllEntities();
        builder.ApplyEntityIds(options);
        builder.ApplyEntityDating(options);
        foreach (Assembly dataAssembly in options.DataAssemblies)
        {
            if (options.ApplyDataSeeders)
                builder.ApplyDataSeedersFromAssembly(dataAssembly);
            if (options.ApplyEntityTypeConfigurations)
                builder.ApplyConfigurationsFromAssembly(dataAssembly);
        }
    }

    /// <summary>
    ///   
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    public static void ApplyEntityIds(this ModelBuilder builder, DbDesignOptions options)
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
                if (options.PreferSqlSideDefaultValues && options.EngineStrategy is DbEngineStrategy.SqlServer)
                    idPropertyBuilder.HasDefaultValueSql(options.SequentialGuids ? "NEWSEQUENTIALID()" : "NEWID()");
                else
                    idPropertyBuilder.HasDefaultValue(Guid.NewGuid());
            }
            else if (entityIdType == typeof(string))
            {
                idPropertyBuilder.HasDefaultValue(Guid.NewGuid().ToString());
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterAllEntities(this ModelBuilder builder)
    {
        foreach (Type entityType in AssemblyProvider.GetImplementationsOf<IEntity>())
        {
            builder.Entity(entityType);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configureOptions"></param>
    public static void ApplyEntityDating(this ModelBuilder builder, Action<DbDesignOptions>? configureOptions = null)
    {
        var options = new DbDesignOptions();
        configureOptions?.Invoke(options);
        builder.ApplyEntityDating(options);
    }

    private static void ApplyEntityDating(this ModelBuilder builder, DbDesignOptions options)
    {
        bool utc = options.DateTimeKind is DateTimeKind.Utc;
        string defaultValueSql = options.EngineStrategy switch
        {
            DbEngineStrategy.SqlServer => utc ? "SYSUTCDATETIME()" : "SYSDATETIME()",
            DbEngineStrategy.Sqlite => utc ? "DATETIME('now')" : "DATETIME()",
            DbEngineStrategy.Postgres => utc ? "TIMEZONE('utc', NOW())" : "NOW()",
            _ => throw new ArgumentException($"{nameof(DbEngineStrategy)} of {options.EngineStrategy} is not currently supported.")
        };

        foreach (Type creatableEntityType in AssemblyProvider.GetImplementationsOf<ICreatableEntity>())
        {
            builder.Entity(creatableEntityType)
                .Property(nameof(ICreatableEntity.Created))
                .HasDefaultValueSql(defaultValueSql);
        }

        foreach (Type updatableEntityType in AssemblyProvider.GetImplementationsOf<IUpdatableEntity>())
        {
            builder.Entity(updatableEntityType)
                .Property(nameof(IUpdatableEntity.Updated))
                .ValueGeneratedOnUpdate()
                .HasComputedColumnSql(defaultValueSql);
        }
    }

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
        builder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }

    /// <param name="assemblyName"><see cref="Assembly"/> name where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyAllDataSeeders"/>
    public static void ApplyDataSeedersFromAssembly(this ModelBuilder builder, string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    /// <param name="assembly"><see cref="Assembly"/> where data seeders are providing.</param>
    /// <inheritdoc cref="ApplyAllDataSeeders"/>
    public static void ApplyDataSeedersFromAssembly(this ModelBuilder builder, Assembly assembly)
    {
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    [Obsolete("Use 'ApplyAllDataSeeders' instead of this.")]
    public static void ApplyDataSeeders(this ModelBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
    }

    /// <summary>
    ///   Applies all <see cref="IEntityDataSeeder{TEntity}"/> and <see cref="IDataSeeder"/>
    ///   seeding data from provided assembly or from the calling assembly by default.
    /// </summary>
    /// <param name="builder">Database model builder.</param>
    public static void ApplyAllDataSeeders(this ModelBuilder builder)
    {
        var assembly = Assembly.GetCallingAssembly();
        ApplyEntityDataSeeders(builder, assembly);
        ApplyExtendedDataSeeders(builder, assembly);
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
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NeerCore.Json;

namespace NeerCore.Data.EntityFramework.Design;

/// <summary>
///   A factory for creating derived <see cref="DbContext" /> instances.
/// </summary>
/// <remarks>
///   See Implementation of database providers and extensions
///   <a href="https://aka.ms/efcore-docs-providers">official documentation</a>
///   for more information.
/// </remarks>
public abstract class DbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    /// <summary>
    ///   Connection string name in settings file.
    /// </summary>
    public virtual string SelectedConnectionName => "DefaultConnection";

    /// <summary>
    ///   Path from the root JSON node to the selected connection string.
    /// </summary>
    /// <remarks>
    ///   Use ':' character as nesting separator.
    /// </remarks>
    public virtual string ConnectionStringsSectionPath => "ConnectionStrings";

    /// <summary>
    ///   Relative path to the json file with connection string configuration.
    /// </summary>
    public virtual string SettingsPath => "appsettings.json";

    /// <summary>
    ///   Connection string used for <see cref="TContext" />.
    /// </summary>
    public virtual string ConnectionString => GetConnectionStringsFromJson(SettingsPath)[SelectedConnectionName];

    /// <summary>
    ///   Path to the assembly with migrations.
    /// </summary>
    public virtual string MigrationsAssembly => GetType().Assembly.GetName().Name!;

    /// <summary>
    ///   Creates a new instance of a derived context.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of db context.</returns>
    public abstract TContext CreateDbContext(string[] args);

    /// <summary>
    ///   Configures DbContext options.
    /// </summary>
    public abstract void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder);

    /// <summary>
    ///   Creates DbContext options.
    /// </summary>
    public virtual DbContextOptions CreateContextOptions()
    {
        Console.WriteLine("Database connection used: " + SelectedConnectionName);

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        ConfigureContextOptions(optionsBuilder);
        return optionsBuilder.Options;
    }

    /// <summary>
    ///   Gets a dictionary of connection strings.
    /// </summary>
    /// <param name="appsettingsPath">Relative path to the json file with connection string configuration.</param>
    /// <exception cref="FileNotFoundException">Throws when configuration does not exist or just not found.</exception>
    /// <exception cref="KeyNotFoundException">
    ///   Throws when configuration file found but <see cref="SelectedConnectionName"/> or <see cref="ConnectionStringsSectionPath"/>
    ///   contains invalid path to the connection strings configuration sections.
    /// </exception>
    /// <returns>Dictionary of all available connection strings.</returns>
    protected virtual IReadOnlyDictionary<string, string> GetConnectionStringsFromJson(string appsettingsPath = "appsettings.json")
    {
        string absPath = Path.Combine(Directory.GetCurrentDirectory(), appsettingsPath);

        if (!File.Exists(absPath))
            throw new FileNotFoundException($"Configuration file not found: '{appsettingsPath}'");

        string json = File.ReadAllText(absPath);
        var settings = JsonSerializer.Deserialize<JsonElement>(json, JsonConventions.ExtendedScheme);
        var pathItems = ConnectionStringsSectionPath.Split(':');

        try
        {
            return pathItems.Aggregate(settings, (current, t) => current.GetProperty(t))
                .EnumerateObject().Where(s => !string.IsNullOrEmpty(s.Value.GetString()))
                .ToDictionary(k => k.Name, v => v.Value.GetString()!);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Configuration file '{appsettingsPath}' successfully found, but no connection string found there by path: '{ConnectionStringsSectionPath}'.");
        }
    }
}
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NeerCore.Exceptions;
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
    private string? _cachedConnectionString;

    /// <summary>
    ///   Enables (true) or disables (false) an internal logging using <see cref="Log"/> method.
    /// </summary>
    /// <remarks>
    ///   You can override <see cref="Log"/> method if you want to use a different logger implementation.
    /// </remarks>
    public virtual TextWriter? LogWriter => Console.Out;

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
    [Obsolete($"Use '{nameof(SettingsPaths)}' property instead. ALREADY DO NOT USED!")]
    public virtual string? SettingsPath => null;

    /// <summary>
    ///   An array of relative paths to the json files with connection string configuration.
    /// </summary>
    /// <remarks>
    ///   If first file not found, the second one will be used, etc.
    /// </remarks>
    public virtual string[] SettingsPaths => new[] { "appsettings.json" };

    /// <summary>
    ///   Connection string used for <see cref="TContext" />.
    /// </summary>
    public virtual string ConnectionString
    {
        get
        {
            _cachedConnectionString ??= GetSelectedConnectionStringFromPaths();
            LogWriter?.Write("Selected connection string: " + _cachedConnectionString + "\n");
            return _cachedConnectionString;
        }
    }

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
    ///
    /// </summary>
    /// <returns></returns>
    public virtual TContext CreateDbContext() => CreateDbContext(null!);

    /// <summary>
    ///   Configures DbContext options.
    /// </summary>
    public abstract void ConfigureContextOptions(DbContextOptionsBuilder optionsBuilder);

    /// <summary>
    ///   Creates DbContext options.
    /// </summary>
    public virtual DbContextOptions<TContext> CreateContextOptions()
    {
        LogWriter?.Write("Database connection used: " + SelectedConnectionName);

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
            throw new KeyNotFoundException($"Configuration file '{appsettingsPath}' successfully found, "
                + $"but no connection string found there by path: '{ConnectionStringsSectionPath}'.");
        }
    }

    /// <summary>
    ///   Gets a dictionary of connection strings or returns null if config file or config key not found.
    /// </summary>
    /// <remarks>
    ///   <b>This method works only with ABSOLUTE path strings!</b>
    /// </remarks>
    /// <param name="appsettingsPath">Relative path to the json file with connection string configuration.</param>
    /// <param name="selectedConnectionName"></param>
    /// <exception cref="FileNotFoundException">Throws when configuration does not exist or just not found.</exception>
    /// <exception cref="KeyNotFoundException">
    ///   Throws when configuration file found but <see cref="SelectedConnectionName"/> or <see cref="ConnectionStringsSectionPath"/>
    ///   contains invalid path to the connection strings configuration sections.
    /// </exception>
    /// <returns>Dictionary of all available connection strings.</returns>
    private string GetSelectedConnectionStringFromPaths()
    {
        foreach (var settingsPath in SettingsPaths)
        {
            var absPath = settingsPath;
            if (!File.Exists(absPath))
            {
                absPath = Path.Combine(Directory.GetCurrentDirectory(), absPath);
                if (!File.Exists(absPath))
                {
                    LogWriter?.Write($"Configuration file not found: '{absPath}'");
                    continue;
                }
            }

            var jsonString = File.ReadAllText(absPath);
            var settings = JsonSerializer.Deserialize<JsonElement>(jsonString, JsonConventions.ExtendedScheme);
            var pathItems = ConnectionStringsSectionPath.Split(':');

            try
            {
                var jsonElement = pathItems.Aggregate(settings, (current, item) => current.GetProperty(item));
                var selectedConnectionString = jsonElement.GetProperty(SelectedConnectionName).GetString();
                if (string.IsNullOrEmpty(selectedConnectionString))
                    continue;
                return selectedConnectionString;
            }
            catch (Exception)
            {
                LogWriter?.Write($"Configuration file '{absPath}' successfully found, "
                    + $"but no connection string found there by path: '{ConnectionStringsSectionPath}'.");
            }
        }

        throw new NotFoundException($"No connection string found in '{nameof(SettingsPaths)}'.\n"
            + $"(Expected the JSON file and '{ConnectionStringsSectionPath}' section with key '{SelectedConnectionName}' within)");
    }
}
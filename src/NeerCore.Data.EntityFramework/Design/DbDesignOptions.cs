using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;

namespace NeerCore.Data.EntityFramework.Design;

/// <summary>
///   Defines an options to configure entity models registration and configuring.
/// </summary>
public class DbDesignOptions
{
    /// <summary>
    ///   Gets or sets DB Engine strategy used for models configuring.
    /// </summary>
    /// <remarks>Default: <b>SqlServer</b></remarks>
    public DbEngineStrategy EngineStrategy { get; set; } = DbEngineStrategy.SqlServer;

    /// <summary>
    ///   If <b>true</b>, whenever possible, it is preferable to generate the value on
    ///   the database side, otherwise it is preferable to generate on the code side. 
    /// </summary>
    public bool PreferSqlSideDefaultValues { get; set; } = true;

    /// <summary>
    ///   Gets or sets value that enables (<b>true</b>) or disables
    ///   (<b>false</b>) sequential GUID generation instead of default.
    /// </summary>
    /// <remarks>Default: <b>false</b></remarks>
    public bool SequentialGuids { get; set; } = false;

    /// <summary>
    ///   Specifies whether a <see cref="DateTime"/> object represents a local time,
    ///   a Coordinated Universal Time (UTC), or is not specified as either local time or UTC.
    /// </summary>
    /// <remarks>Default: <b>Local</b></remarks>
    public DateTimeKind DateTimeKind { get; set; } = DateTimeKind.Local;

    /// <summary>
    ///   Gets or sets value that enables (<b>true</b>) or disables
    ///   (<b>false</b>) entities configuring with <see cref="IEntityTypeConfiguration{TEntity}" /> interface.
    /// </summary>
    /// <remarks>Default: <b>true</b></remarks>
    public bool ApplyEntityTypeConfigurations { get; set; } = true;

    /// <summary>
    ///   Gets or sets value that enables (<b>true</b>) or disables
    ///   (<b>false</b>) NeerCore data seeding with <see cref="IEntityDataSeeder{TEntity}"/>
    ///   and <see cref="IDataSeeder"/> interface.
    /// </summary>
    /// <remarks>Default: <b>true</b></remarks>
    public bool ApplyDataSeeders { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    public Assembly[]? DataAssemblies { get; set; }
}
using System.Data;

namespace NeerCore.Logging.Settings;

/// <summary>
///   Disables specific logger if <b>false</b> or enables if <b>true</b>
///   (<b>true</b> by default).
/// </summary>
public sealed class DatabaseTargetSettings
{
    /// <summary>
    ///   Disables specific logger if <b>false</b> or enables if <b>true</b>
    ///   (<b>false</b> by default).
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///   Gets or sets the connection string. When provided, it overrides the values
    ///   specified in DBHost, DBUserName, DBPassword, DBDatabase.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? TableSchema { get; set; } = "dbo";

    /// <summary>
    ///
    /// </summary>
    public string? TableName { get; set; } = "ServerLogs";

    /// <summary>
    ///   Gets or sets the name of the database provider.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   The parameter name should be a provider invariant name as registered in machine.config or app.config. Common values are:
    /// </para>
    /// <ul>
    ///   <li><c>System.Data.SqlClient</c> - <see href="https://msdn.microsoft.com/en-us/library/system.data.sqlclient.aspx">SQL Sever Client</see></li>
    ///   <li><c>System.Data.SqlServerCe.3.5</c> - <see href="https://www.microsoft.com/sqlserver/2005/en/us/compact.aspx">SQL Sever Compact 3.5</see></li>
    ///   <li><c>System.Data.OracleClient</c> - <see href="https://msdn.microsoft.com/en-us/library/system.data.oracleclient.aspx">Oracle Client from Microsoft</see> (deprecated in .NET Framework 4)</li>
    ///   <li><c>Oracle.DataAccess.Client</c> - <see href="https://www.oracle.com/technology/tech/windows/odpnet/index.html">ODP.NET provider from Oracle</see></li>
    ///   <li><c>System.Data.SQLite</c> - <see href="http://sqlite.phxsoftware.com/">System.Data.SQLite driver for SQLite</see></li>
    ///   <li><c>Npgsql</c> - <see href="https://www.npgsql.org/">Npgsql driver for PostgreSQL</see></li>
    ///   <li><c>MySql.Data.MySqlClient</c> - <see href="https://www.mysql.com/downloads/connector/net/">MySQL Connector/Net</see></li>
    /// </ul>
    /// <para>
    ///   (Note that provider invariant names are not supported on .NET Compact Framework).
    /// </para>
    /// <para>
    ///   Alternatively the parameter value can be be a fully qualified name of the provider
    ///   connection type (class implementing <see cref="IDbConnection" />) or one of the following tokens:
    /// </para>
    /// <ul>
    ///   <li><c>sqlserver</c>, <c>mssql</c>, <c>microsoft</c> or <c>msde</c> - SQL Server Data Provider</li>
    ///   <li><c>oledb</c> - OLEDB Data Provider</li>
    ///   <li><c>odbc</c> - ODBC Data Provider</li>
    /// </ul>
    /// </remarks>
    public string DbProvider { get; set; } = "sqlserver";

    /// <summary>
    ///   Gets or sets a value indicating whether to keep the
    ///   database connection open between the log events.
    /// </summary>
    public bool KeepConnection { get; set; }

    /// <summary>
    ///   Configures isolated transaction batch writing.
    ///   If supported by the database, then it will improve insert performance.
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }

    /// <summary>
    ///   Gets or sets the text of the SQL command to be run on each log level.
    /// </summary>
    /// <remarks>
    ///   Typically this is a SQL INSERT statement or a stored procedure call.
    ///   It should use the database-specific parameters (marked as <c>@parameter</c>
    ///   for SQL server or <c>:parameter</c> for Oracle, other data providers
    ///   have their own notation) and not the layout renderers,
    ///   because the latter is prone to SQL injection attacks.
    ///   The layout renderers should be specified as &lt;parameter /&gt; elements instead.
    /// </remarks>
    public string? CommandText { get; set; }

    /// <summary>
    ///   Gets or sets the type of the SQL command to be run on each log level.
    /// </summary>
    /// <remarks>
    ///   This specifies how the command text is interpreted, as "Text" (default) or as "StoredProcedure".
    ///   When using the value StoredProcedure, the commandText-property would
    ///   normally be the name of the stored procedure. TableDirect method is not supported in this context.
    /// </remarks>
    public CommandType CommandType { get; set; } = CommandType.Text;

    /// <summary>
    ///   If <b>true</b> it will make sure that logs table exist in provided DB.
    /// </summary>
    public bool EnsureTableCreated { get; set; } = true;

    /// <summary>
    ///   If <b>true</b> it will add some extra rows to DB with ASP.NET specific data.
    /// </summary>
    public bool AspNetFeatures { get; set; } = false;
}
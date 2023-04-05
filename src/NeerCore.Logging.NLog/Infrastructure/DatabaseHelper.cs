using System.Data;
using NeerCore.Logging.Settings;
using NLog.Config;
using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public static class DatabaseHelper
{
    public static string GetInsertLogQuery(DatabaseTargetSettings targetSettings)
    {
        if (IsSqlServer(targetSettings))
        {
            var table = $"[{targetSettings.TableSchema}].[{targetSettings.TableName}]";
            return targetSettings.AspNetFeatures
                ? $@"insert into {table} (Created, Level, Logger, Message, Exception, RequestUrl, Ip, UserAgent)
                    values (@timestamp, @level, @logger, @message, @exception, @url, @ip, @useragent)"
                : $@"insert into {table} (Created, Level, Logger, Message, Exception)
                    values (@timestamp, @level, @logger, @message, @exception)";
        }
        if (IsPostgreSql(targetSettings))
        {
            var table = $@"""{targetSettings.TableSchema}"".""{targetSettings.TableName}""";
            return targetSettings.AspNetFeatures
                ? $@"insert into {table} (""Created"", ""Level"", ""Logger"", ""Message"", ""Exception"", ""RequestUrl"", ""Ip"", ""UserAgent"")
                    values (@timestamp::timestamp, @level, @logger, @message, @exception, @url, @ip, @useragent)"
                : $@"insert into {table} (""Created"", ""Level"", ""Logger"", ""Message"", ""Exception"")
                    values (@timestamp::timestamp, @level, @logger, @message, @exception)";
        }
        throw new NotSupportedException($"Db provider '{targetSettings.DbProvider}' is not supported yet. "
            + "Please contact a developer on package github repo: https://github.com/neerspace/NeerCore");
    }

    public static void EnsureDbCreated(DatabaseTarget target, DatabaseTargetSettings targetSettings)
    {
        if (IsSqlServer(targetSettings))
            EnsureSqlServerCreated(target, targetSettings);
        else if (IsPostgreSql(targetSettings))
            EnsurePostgresCreated(target, targetSettings);

        // we can now connect to the target DB
        target.InstallConnectionString = targetSettings.ConnectionString;

        // create the table if it does not exist
        var installationContext = new InstallationContext();
        target.Install(installationContext);
    }

    /// <summary>
    /// Source: https://stackoverflow.com/questions/20101809/creating-a-database-programatically-in-nlog-to-enable-using-databasetarget
    /// </summary>
    private static void EnsureSqlServerCreated(DatabaseTarget target, DatabaseTargetSettings targetSettings)
    {
        if (string.IsNullOrEmpty(targetSettings.TableSchema))
            targetSettings.TableSchema = "dbo";
        if (string.IsNullOrEmpty(targetSettings.TableName))
            targetSettings.TableName = "ServerLogs";


        var aspNetExtraProps = targetSettings.AspNetFeatures
            ? @",
[Ip]         varchar(40),
[UserAgent]  nvarchar(500),
[RequestUrl] nvarchar(500)"
            : "";

        target.InstallDdlCommands.Add(new DatabaseCommandInfo
        {
            Text = @$"if exists (select * from INFORMATION_SCHEMA.TABLES
where TABLE_SCHEMA = '{targetSettings.TableSchema}' and TABLE_NAME = '{targetSettings.TableName}')
    return
create table [{targetSettings.TableSchema}].[{targetSettings.TableName}](
[Created]    datetime2     not null,
[Level]      varchar(8)    not null,
[Logger]     varchar(200)  not null,
[Message]    nvarchar(max) not null,
[Exception]  nvarchar(max)
    {aspNetExtraProps})",
            CommandType = CommandType.Text,
        });
    }

    private static void EnsurePostgresCreated(DatabaseTarget target, DatabaseTargetSettings targetSettings)
    {
        if (string.IsNullOrEmpty(targetSettings.TableSchema))
            targetSettings.TableSchema = "public";
        if (string.IsNullOrEmpty(targetSettings.TableName))
            targetSettings.TableName = "server_logs";

        var aspNetExtraProps = targetSettings.AspNetFeatures
            ? @",
""Ip""         varchar(40),
""UserAgent""  varchar(500),
""RequestUrl"" varchar(500)"
            : "";

        target.InstallDdlCommands.Add(new DatabaseCommandInfo
        {
            Text = $@"create table if not exists ""{targetSettings.TableSchema}"".""{targetSettings.TableName}""(
""Created""   timestamp    not null,
""Level""     varchar(8)   not null,
""Logger""    varchar(200) not null,
""Message""   text         not null,
""Exception"" text
    {aspNetExtraProps})",
            CommandType = CommandType.Text,
        });
    }

    private static bool IsPostgreSql(DatabaseTargetSettings targetSettings) =>
        targetSettings.DbProvider.Contains("Npgsql", StringComparison.OrdinalIgnoreCase)
        || targetSettings.DbProvider.Contains("Postgres", StringComparison.OrdinalIgnoreCase);

    private static bool IsSqlServer(DatabaseTargetSettings targetSettings) =>
        targetSettings.DbProvider.Contains("SqlServer", StringComparison.OrdinalIgnoreCase);
}
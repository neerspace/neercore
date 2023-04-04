using System.Data;
using NLog.Config;
using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class DatabaseTargetBuilder : TargetBuilderBase
{
    public override bool Enabled => Settings.Targets.Database.Enabled;


    public override Target Build()
    {
        var targetSettings = Settings.Targets.Database;

        var insertCommand = targetSettings.AspNetFeatures
            ? $@"insert into {targetSettings.TableName}
(Created, Level, Logger, Message, Exception, RequestUrl, Ip, UserAgent) values
(@timestamp, @level, @logger, @message, @exception, @url, @ip, @useragent)"
            : $@"insert into {targetSettings.TableName}
(Created, Level, Logger, Message, Exception) values
(@timestamp, @level, @logger, @message, @exception)";

        var target = new DatabaseTarget("logDatabase")
        {
            DBProvider = targetSettings.DbProvider,
            ConnectionString = targetSettings.ConnectionString,
            KeepConnection = targetSettings.KeepConnection,
            CommandText = targetSettings.CommandText ?? insertCommand,
            CommandType = targetSettings.CommandType,
            IsolationLevel = targetSettings.IsolationLevel,
            Parameters =
            {
                new DatabaseParameterInfo("@timestamp", "${longdate}"),
                new DatabaseParameterInfo("@level", "${level}"),
                new DatabaseParameterInfo("@logger", "${logger}"),
                new DatabaseParameterInfo("@message", "${message}"),
                new DatabaseParameterInfo("@exception", "${exception}"),
            }
        };

        if (targetSettings.AspNetFeatures)
        {
            target.Parameters.Add(new DatabaseParameterInfo("@url", "${aspnet-request-url}"));
            target.Parameters.Add(new DatabaseParameterInfo("@ip", "${aspnet-request-ip}"));
            target.Parameters.Add(new DatabaseParameterInfo("@useragent", "${aspnet-request-useragent}"));
        }

        return target;
    }

    public override void Configure(LoggingConfiguration configuration, Target target)
    {
        configuration.AddTarget(target.Name, target);
        ApplyLogLevelsFromSettings(configuration, target);
        EnsureDbCreated((DatabaseTarget)target);
    }

    /// <summary>
    /// Source: https://stackoverflow.com/questions/20101809/creating-a-database-programatically-in-nlog-to-enable-using-databasetarget
    /// </summary>
    protected void EnsureDbCreated(DatabaseTarget target)
    {
        var targetSettings = Settings.Targets.Database;
        if (!targetSettings.EnsureCreated)
            return;

        if (string.IsNullOrEmpty(targetSettings.TableSchema))
            targetSettings.TableSchema = "dbo";
        if (string.IsNullOrEmpty(targetSettings.TableName))
            targetSettings.TableName = "ServerLogs";

        var installationContext = new InstallationContext();

        var aspNetExtraProps = targetSettings.AspNetFeatures
            ? @",
[Ip]         varchar(40),
[UserAgent]  nvarchar(500),
[RequestUrl] nvarchar(500)"
            : "";

        target.InstallDdlCommands.Add(new DatabaseCommandInfo
        {
            Text = @$"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = '{targetSettings.TableSchema}' AND TABLE_NAME = '{targetSettings.TableName}')
RETURN
    CREATE TABLE [{targetSettings.TableSchema}].[{targetSettings.TableName}](
    [Created]    datetime2     not null,
    [Level]      varchar(8)    not null,
    [Logger]     varchar(200)  not null,
    [Message]    nvarchar(max) not null,
    [Exception]  nvarchar(max)
    {aspNetExtraProps})",
            CommandType = CommandType.Text,
        });

        // we can now connect to the target DB
        target.InstallConnectionString = targetSettings.ConnectionString;

        // create the table if it does not exist
        target.Install(installationContext);
    }
}
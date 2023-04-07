using NLog.Targets;

namespace NeerCore.Logging.Infrastructure;

public class DatabaseTargetBuilder : TargetBuilderBase
{
    public override bool Enabled => Settings.Targets.Database.Enabled;


    public override Target Build()
    {
        var targetSettings = Settings.Targets.Database;

        var insertCommand = DatabaseHelper.GetInsertLogQuery(targetSettings);

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
                new DatabaseParameterInfo("@timestamp", "${date:universalTime=true}"),
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

        if (targetSettings.EnsureTableCreated)
            DatabaseHelper.EnsureDbCreated(target, targetSettings);

        return target;
    }
}
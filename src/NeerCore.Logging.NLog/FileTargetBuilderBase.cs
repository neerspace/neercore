using System.Text.RegularExpressions;

namespace NeerCore.Logging;

public abstract class FileTargetBuilderBase : TargetBuilderBase
{
    protected virtual string FileLayout { get; set; } =
        "${longdate} |${level:uppercase=true:truncate=4}| — ${logger}[${threadid}]\n${message} ${exception:format=ToString}";


    protected string BuildLogFilePath(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentNullException(nameof(fileName), "File name is not invalid.");

        string path = Settings.Shared.LogsDirectoryPath + fileName;

        if (path.Contains('~'))
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "/";
            path = path.Replace("~", basePath)
                .Replace("//", string.Empty)
                .Replace("//", string.Empty);
        }

        int iter = 100;
        const string pathRegex = @"((?!\.)[^\/]+\/)\.\.\/";
        while (path.Contains("../") && iter-- > 0)
            path = Regex.Replace(path, pathRegex, "");

        if (iter == 0)
            throw new LockRecursionException("Cannot parse path.");

        return path;
    }
}
using NeerCore.DependencyInjection;

namespace AsmTest.Root;

public interface IRootService
{
    void Log();
}

[Service<IRootService>]
internal class RootService : IRootService
{
    public void Log() => Console.WriteLine(nameof(RootService));
}
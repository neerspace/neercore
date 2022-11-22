using NeerCore.DependencyInjection;

namespace AsmTest.Root;

[Dependency]
public class RootService
{
    public void Log() => Console.WriteLine(nameof(RootService));
}
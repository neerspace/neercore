using NeerCore.DependencyInjection;

namespace AsmTest.Root.SecondLvl;

[Dependency]
public class SecondService
{
    public void Log() => Console.WriteLine(nameof(SecondService));
}
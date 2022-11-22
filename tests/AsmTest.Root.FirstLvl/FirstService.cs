using NeerCore.DependencyInjection;

namespace AsmTest.Root.FirstLvl;

[Dependency]
public class FirstService
{
    public void Log() => Console.WriteLine(nameof(FirstService));
}
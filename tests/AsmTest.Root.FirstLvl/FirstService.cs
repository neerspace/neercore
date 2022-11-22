using NeerCore.DependencyInjection;

namespace AsmTest.Root.FirstLvl;

[Service(Lifetime = Lifetime.Scoped | Lifetime.Singleton)]
public class FirstService
{
    public void Log() => Console.WriteLine(nameof(FirstService));
}
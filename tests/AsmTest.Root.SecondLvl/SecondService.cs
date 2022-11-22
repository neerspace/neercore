using NeerCore.DependencyInjection;

namespace AsmTest.Root.SecondLvl;

[Service]
public class SecondService
{
    public void Log() => Console.WriteLine(nameof(SecondService));
}
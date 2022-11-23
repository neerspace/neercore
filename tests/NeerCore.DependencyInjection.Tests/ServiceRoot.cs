namespace NeerCore.DependencyInjection.Tests;

public interface IServiceRoot
{
    bool Test();
}

[Service]
internal class ServiceRoot : IServiceRoot
{
    public bool Test() => true;
}
using NeerCore.DependencyInjection;

namespace NeerCore1.DependencyInjection.TestsSideAssembly;

[Service]
public class Service1
{
    public bool Test() => true;
}

public interface IService1AsInterface
{
    bool Test();
}

[Service]
public class Service1AsInterface : IService1AsInterface
{
    public bool Test() => true;
}
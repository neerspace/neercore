namespace NeerCore.DependencyInjection.TestsSideAssembly;

public abstract class ServiceBase
{
    public abstract bool Test();
}

[Service(InjectionType = InjectionType.BaseClass)]
public class ServiceX : ServiceBase
{
    public override bool Test() => true;
}
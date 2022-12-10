using NeerCore.DependencyInjection.Extensions;

namespace NeerCore.DependencyInjection.Tests.Tests.Extensions;

public sealed class TypeExtensionsTests
{
    private interface IInterface { }

    [Service]
    private class MyClass : IInterface { }

    private sealed class MyClassChild : MyClass { }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class TestAttribute : Attribute { }

    [Fact]
    public void GetAttribute_Test()
    {
        var type = typeof(MyClass);
        var childType = typeof(MyClassChild);

        Attribute? attr = type.GetAttribute<ServiceAttribute>();
        Assert.NotNull(attr);

        attr = childType.GetAttribute<ServiceAttribute>(searchInParents: true);
        Assert.NotNull(attr);

        attr = type.GetAttribute<TestAttribute>();
        Assert.Null(attr);
    }

    [Fact]
    public void GetRequiredAttribute_Test()
    {
        var type = typeof(MyClass);
        var childType = typeof(MyClassChild);

        Attribute attr = type.GetRequiredAttribute<ServiceAttribute>();
        Assert.NotNull(attr);

        attr = childType.GetRequiredAttribute<ServiceAttribute>(searchInParents: true);
        Assert.NotNull(attr);

        Assert.Throws<TypeLoadException>(() => type.GetRequiredAttribute<TestAttribute>());
    }

    [Fact]
    public void InheritsFrom_Test()
    {
        var interf = typeof(IInterface);
        var type = typeof(MyClass);
        var childType = typeof(MyClassChild);

        Assert.True(type.InheritsFrom(interf));
        Assert.True(childType.InheritsFrom(interf));
        Assert.True(childType.InheritsFrom(type));

        Assert.False(type.InheritsFrom(childType));
        Assert.False(interf.InheritsFrom(type));
    }
}
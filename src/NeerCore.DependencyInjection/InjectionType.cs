namespace NeerCore.DependencyInjection;

/// <summary>
///   Ways to inject a class into a DI container.
/// </summary>
public enum InjectionType
{
    /// <summary>
    ///   Uses the default value defined at configuring injection.
    /// </summary>
    Default,

    /// <summary>
    ///   Automatically define prefer injection type.
    /// </summary>
    /// <remarks>
    ///    How it works:
    ///    <list type="number">
    ///      <item>If it implements any interface it will be injected as <see cref="Interface"/>.</item>
    ///      <item>If it extends any class it will be injected as <see cref="BaseClass"/>.</item>
    ///      <item>Otherwise it will be injected as <see cref="Self"/>.</item>
    ///    </list>
    /// </remarks>
    Auto,

    /// <summary>
    ///   Inject as implemented interface realisation.
    /// </summary>
    Interface,

    /// <summary>
    ///   Inject own self.
    /// </summary>
    Self,

    /// <summary>
    ///   Inject as parent class.
    /// </summary>
    BaseClass
}
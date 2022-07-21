namespace NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

public interface IMathService
{
    double Pi { get; }
    T Compare<T>(params T[] args) where T : IComparable<T>;
}
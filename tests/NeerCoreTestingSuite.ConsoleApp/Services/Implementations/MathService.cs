using NeerCore.DependencyInjection;
using NeerCoreTestingSuite.ConsoleApp.Services.Abstractions;

namespace NeerCoreTestingSuite.ConsoleApp.Services.Implementations;

[Injectable]
public class MathService : IMathService
{
    public double Pi => Math.PI;

    public T Compare<T>(params T[] args) where T : IComparable<T>
    {
        bool result = true;
        foreach (T item1 in args)
        {
            foreach (T item2 in args)
            {
                // if (item1.CompareTo())
            }
        }

        return default;
    }
}
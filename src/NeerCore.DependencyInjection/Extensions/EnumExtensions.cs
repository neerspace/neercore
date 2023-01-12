using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static class EnumExtensions
{
    public static IEnumerable<ServiceLifetime> ToServiceLifetimes(this Lifetime lifetime)
    {
        if (lifetime.HasFlag(Lifetime.Singleton))
            yield return ServiceLifetime.Singleton;
        if (lifetime.HasFlag(Lifetime.Scoped))
            yield return ServiceLifetime.Scoped;
        if (lifetime.HasFlag(Lifetime.Transient))
            yield return ServiceLifetime.Transient;
    }

    public static Lifetime ToInstanceLifetime(this ServiceLifetime serviceLifetime) =>
        serviceLifetime switch
        {
            ServiceLifetime.Singleton => Lifetime.Singleton,
            ServiceLifetime.Scoped    => Lifetime.Scoped,
            ServiceLifetime.Transient => Lifetime.Transient,
            _                         => throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null)
        };
}
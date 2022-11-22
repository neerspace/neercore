using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static class EnumExtensions
{
    public static ServiceLifetime ToServiceLifetime(this Lifetime lifetime)
    {
        return lifetime switch
        {
            Lifetime.Singleton => ServiceLifetime.Singleton,
            Lifetime.Scoped    => ServiceLifetime.Scoped,
            Lifetime.Transient => ServiceLifetime.Transient,
            _                  => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };
    }

    public static Lifetime ToInstanceLifetime(this ServiceLifetime serviceLifetime)
    {
        return serviceLifetime switch
        {
            ServiceLifetime.Singleton => Lifetime.Singleton,
            ServiceLifetime.Scoped    => Lifetime.Scoped,
            ServiceLifetime.Transient => Lifetime.Transient,
            _                         => throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null)
        };
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.DependencyInjection.Extensions;

public static class EnumExtensions
{
    public static ServiceLifetime ToServiceLifetime(this InstanceLifetime instanceLifetime)
    {
        return instanceLifetime switch
        {
            InstanceLifetime.Singleton => ServiceLifetime.Singleton,
            InstanceLifetime.Scoped => ServiceLifetime.Scoped,
            InstanceLifetime.Transient => ServiceLifetime.Transient,
            _ => throw new ArgumentOutOfRangeException(nameof(instanceLifetime), instanceLifetime, null)
        };
    }

    public static InstanceLifetime ToInstanceLifetime(this ServiceLifetime serviceLifetime)
    {
        return serviceLifetime switch
        {
            ServiceLifetime.Singleton => InstanceLifetime.Singleton,
            ServiceLifetime.Scoped => InstanceLifetime.Scoped,
            ServiceLifetime.Transient => InstanceLifetime.Transient,
            _ => throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null)
        };
    }
}
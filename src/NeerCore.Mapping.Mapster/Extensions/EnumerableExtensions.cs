using Mapster;

namespace NeerCore.Mapping.Extensions;

public static class EnumerableExtensions
{
	public static IEnumerable<TDestination> AdaptAll<TDestination>(this IEnumerable<object>? enumerable)
	{
		return enumerable is null
				? Array.Empty<TDestination>()
				: enumerable.Select(o => o.Adapt<TDestination>());
	}
}
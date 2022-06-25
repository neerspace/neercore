using Mapster;

namespace NeerCore.Mapping.Extensions;

public static class EnumerableExtensions
{
	/// <summary>Adapts a sequence of objects with one type to sequence of type <typeparamref name="TDestination"/>.</summary>
	/// <param name="source">The <see cref="IEnumerable{T}"/> to adapt.</param>
	/// <typeparam name="TDestination">Destination sequence items type.</typeparam>
	/// <returns> Sequence of type <typeparamref name="TDestination"/>.</returns>
	public static IEnumerable<TDestination> AdaptAll<TDestination>(this IEnumerable<object>? source)
	{
		return source is null
				? Array.Empty<TDestination>()
				: source.Select(o => o.Adapt<TDestination>());
	}

	/// <inheritdoc cref="AdaptAll{TDestination}"/>
	/// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
	public static IEnumerable<TDestination> AdaptAll<TSource, TDestination>(this IEnumerable<TSource>? source)
	{
		return source is null
				? Array.Empty<TDestination>()
				: source.Select(o => o.Adapt<TSource, TDestination>());
	}
}
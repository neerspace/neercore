using NeerCore.Exceptions;

namespace NeerCore.Extensions;

public static class EnumerableExtensions
{
	public static T FirstOr404<T>(this IEnumerable<T> enumerable)
	{
		return enumerable.FirstOrDefault()
		       ?? throw new NotFoundException(typeof(T).Name.CamelCaseToWords() + " not found.");
	}

	public static T FirstOr404<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		return enumerable.FirstOrDefault(predicate)
		       ?? throw new NotFoundException(typeof(T).Name.CamelCaseToWords() + " not found.");
	}
}
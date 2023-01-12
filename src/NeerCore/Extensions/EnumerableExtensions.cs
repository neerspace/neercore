using NeerCore.Exceptions;

namespace NeerCore.Extensions;

/// <summary>
///
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///   Returns the first element of a sequence, or throws a
    ///   <see cref="NotFoundException"/> if the sequence contains no elements.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
    /// <param name="errorMessage">The error message that will be threw instead of the default one when an exception occurs.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <returns>Found entity or throws 404 exception.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
    /// <exception cref="NotFoundException"><paramref name="source"/> is empty.</exception>
    public static TSource FirstOr404<TSource>(this IEnumerable<TSource> source, string? errorMessage = null) =>
        source.FirstOrDefault() ?? throw CreateNotFoundException<TSource>(errorMessage);

    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <inheritdoc cref="FirstOr404{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static TSource FirstOr404<TSource>(
        this IEnumerable<TSource> source, Func<TSource, bool> predicate, string? errorMessage = null) =>
        source.FirstOrDefault(predicate) ?? throw CreateNotFoundException<TSource>(errorMessage);


    private static NotFoundException CreateNotFoundException<TSource>(string? errorMessage) =>
        string.IsNullOrEmpty(errorMessage)
            ? new NotFoundException<TSource>()
            : new NotFoundException(errorMessage);

    public static void Deconstruct<T>(this IList<T> seq, out T first, out IEnumerable<T> rest)
    {
        if (seq is null)
            throw new ArgumentNullException(nameof(seq));
        if (seq.Count < 1)
            throw DeconstructException();
        first = seq[0];
        rest = seq.Skip(1);
    }

    public static void Deconstruct<T>(this IList<T> seq, out T first, out T second, out IEnumerable<T> rest)
    {
        if (seq is null)
            throw new ArgumentNullException(nameof(seq));
        if (seq.Count < 2)
            throw DeconstructException();
        first = seq[0];
        second = seq[1];
        rest = seq.Skip(2);
    }

    public static void Deconstruct<T>(
        this IList<T> seq, out T first, out T second, out T third,
        out IEnumerable<T> rest)
    {
        if (seq is null)
            throw new ArgumentNullException(nameof(seq));
        if (seq.Count < 3)
            throw DeconstructException();
        first = seq[0];
        second = seq[1];
        third = seq[2];
        rest = seq.Skip(3);
    }

    public static void Deconstruct<T>(
        this IList<T> seq, out T first, out T second, out T third,
        out T fourth, out IEnumerable<T> rest)
    {
        if (seq is null)
            throw new ArgumentNullException(nameof(seq));
        if (seq.Count < 3)
            throw DeconstructException();
        first = seq[0];
        second = seq[1];
        third = seq[2];
        fourth = seq[3];
        rest = seq.Skip(4);
    }

    public static void Deconstruct<T>(
        this IList<T> seq, out T first, out T second, out T third,
        out T fourth, out T fifth,
        out IEnumerable<T> rest)
    {
        if (seq is null)
            throw new ArgumentNullException(nameof(seq));
        if (seq.Count < 3)
            throw DeconstructException();
        first = seq[0];
        second = seq[1];
        third = seq[2];
        fourth = seq[3];
        fifth = seq[4];
        rest = seq.Skip(4);
    }

    private static IndexOutOfRangeException DeconstructException() =>
        new("Trying to deconstruct a sequence where there are not enough elements.");
}
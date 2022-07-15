using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NeerCore.Data.Abstractions;
using NeerCore.Exceptions;

namespace NeerCore.Data.EntityFramework.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    ///   Returns the first element of a sequence, or throws a
    ///    <see cref="NotFoundException"/> if the sequence contains no elements.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <param name="cancel">Cancellation token</param>
    /// <typeparam name="TEntity">The type of the elements of <paramref name="source"/>. Must be a type that implements <see cref="IEntity"/> interface.</typeparam>
    /// <returns>Found entity or throws 404 exception.</returns>
    /// <exception cref="NotFoundException{TEntity}"><paramref name="source"/> is empty.</exception>
    public static async Task<TEntity> FirstOr404Async<TEntity>(this IQueryable<TEntity> source, CancellationToken cancel = default)
        where TEntity : class, IEntity
    {
        return await source.FirstOrDefaultAsync(cancel)
               ?? throw new NotFoundException<TEntity>();
    }

    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <inheritdoc cref="FirstOr404Async{TEntity}(System.Linq.IQueryable{TEntity},System.Threading.CancellationToken)"/>
    public static async Task<TEntity> FirstOr404Async<TEntity>(this IQueryable<TEntity> source, Expression<Func<TEntity, bool>> predicate, CancellationToken cancel = default)
        where TEntity : class, IEntity
    {
        return await source.FirstOrDefaultAsync(predicate, cancel)
               ?? throw new NotFoundException<TEntity>();
    }

    /// <summary>
    ///   Specifies related entities to include in the query results.
    ///   The navigation property to be included is specified starting with
    ///   the type of entity being queried (TEntity). Further navigation
    ///   properties to be included can be appended, separated by the '.' character.
    /// </summary>
    /// <param name="source">The source query.</param>
    /// <param name="inclusions">A list of strings of '.' separated navigation property names to be included.</param>
    /// <typeparam name="TEntity">Entity type that implements <see cref="IEntity"/> interface.</typeparam>
    public static IQueryable<TEntity> IncludeMany<TEntity>(this IQueryable<TEntity> source, params string[] inclusions)
        where TEntity : class, IEntity
    {
        if (inclusions.Length <= 0)
            return source;

        foreach (string inclusion in inclusions)
            source = source.Include(inclusion);
        return source;
    }
}
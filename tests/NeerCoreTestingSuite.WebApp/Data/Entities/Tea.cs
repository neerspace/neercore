using NeerCore.Data;
using NeerCore.Data.Abstractions;
using Sieve.Attributes;

namespace NeerCoreTestingSuite.WebApp.Data.Entities;

public class Tea : IDateableEntity<Guid>
{
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid Id { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    public LocalizedString Name { get; init; }

    [Sieve(CanFilter = true, CanSort = true, Name = "priceUSD")]
    public decimal Price { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? Updated { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime Created { get; init; }
}
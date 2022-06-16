using NeerCore.Data.Abstractions;
using Sieve.Attributes;

namespace JuniorTemplate.Data.Entities;

public class Tea : IDatedEntity<Guid>
{
	[Sieve(CanFilter = true, CanSort = true)]
	public Guid Id { get; init; } = Guid.NewGuid();

	[Sieve(CanFilter = true, CanSort = true)]
	public string Name { get; init; } = default!;

	[Sieve(CanFilter = true, CanSort = true, Name = "priceUSD")]
	public decimal Price { get; init; }

	[Sieve(CanFilter = true, CanSort = true)]
	public DateTime? Updated { get; init; }

	[Sieve(CanFilter = true, CanSort = true)]
	public DateTime Created { get; init; } = DateTime.UtcNow;
}
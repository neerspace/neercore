using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NeerCore.Data.Abstractions;
using Sieve.Attributes;

namespace NeerCoreTestingSuite.WebApp.Data.Entities;

[Table("Teas")]
public class Tea : IDatedEntity<Guid>
{
    [Sieve(CanFilter = true, CanSort = true)]
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Sieve(CanFilter = true, CanSort = true)]
    public NeerCore.LocalizedString Name { get; init; } = default!;

    [Sieve(CanFilter = true, CanSort = true, Name = "priceUSD")]
    public decimal Price { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? Updated { get; init; }

    [Sieve(CanFilter = true, CanSort = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime Created { get; init; } = DateTime.UtcNow;
}
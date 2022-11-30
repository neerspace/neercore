using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NeerCore.Data.Abstractions;

namespace EFTest.SqlServer.Entities;

[Table("Animals")]
public sealed class Animal : IEntity<AnimalId>
{
    [Key]
    public AnimalId Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Species { get; set; } = default!;

    public override string ToString() => $"{Id.Value} => {Species}";
}
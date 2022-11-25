using NeerCore.Data.Abstractions;

namespace EFTest.SqlServer.Entities;

public class Animal : IEntity<int>
{
    public int Id { get; set; }
    public string Species { get; set; } = default!;
}
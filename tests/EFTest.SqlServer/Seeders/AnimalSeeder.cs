using EFTest.SqlServer.Entities;
using NeerCore.Data.EntityFramework.Abstractions;

namespace EFTest.SqlServer.Seeders;

public sealed class AnimalSeeder : IEntityDataSeeder<Animal>
{
    public IEnumerable<Animal> Data => new[]
    {
        new Animal
        {
            Id = new AnimalId(1),
            Species = "Cat"
        },
        new Animal
        {
            Id = new AnimalId(2),
            Species = "Dog"
        },
        new Animal
        {
            Id = new AnimalId(3),
            Species = "Monkey"
        },
        new Animal
        {
            Id = new AnimalId(4),
            Species = "Chicken"
        },
    };
}
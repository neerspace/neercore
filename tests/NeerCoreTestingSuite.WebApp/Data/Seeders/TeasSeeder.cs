using NeerCore.Data.EntityFramework.Abstractions;
using NeerCoreTestingSuite.WebApp.Data.Entities;

namespace NeerCoreTestingSuite.WebApp.Data.Seeders;

public class TeasSeeder : IEntityDataSeeder<Tea>
{
    public IEnumerable<Tea> Data => new Tea[]
    {
        new() { Id = Guid.NewGuid(), Name = "Earl Gray", Price = 20m },
        new() { Id = Guid.NewGuid(), Name = "Rose Tea", Price = 20m },
        new() { Id = Guid.NewGuid(), Name = "English Breakfast Tea", Price = 18m },
        new() { Id = Guid.NewGuid(), Name = "Big Sur Tea", Price = 25m },
        new() { Id = Guid.NewGuid(), Name = "Big Sur Tea", Price = 25m },
        new() { Id = Guid.NewGuid(), Name = "Jasmine Pearls", Price = 41m },
        new() { Id = Guid.NewGuid(), Name = "Dragonwell", Price = 30m },
        new() { Id = Guid.NewGuid(), Name = "White Peach Tea", Price = 29m },
        new() { Id = Guid.NewGuid(), Name = "Vanilla Berry Tea", Price = 21m },
        new() { Id = Guid.NewGuid(), Name = "Chaga Chai Mushroom Tea", Price = 20m },
        new() { Id = Guid.NewGuid(), Name = "Naked Pu-erh Tea", Price = 27m },
    };
}
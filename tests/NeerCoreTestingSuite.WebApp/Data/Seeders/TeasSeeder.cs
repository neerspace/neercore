using NeerCore.Data.EntityFramework.Abstractions;
using NeerCoreTestingSuite.WebApp.Data.Entities;

namespace NeerCoreTestingSuite.WebApp.Data.Seeders;

public class TeasSeeder : IEntityDataSeeder<Tea>
{
    public IEnumerable<Tea> Data => new Tea[]
    {
        new() { Name = "Earl Gray", Price = 20m },
        new() { Name = "Rose Tea", Price = 20m },
        new() { Name = "English Breakfast Tea", Price = 18m },
        new() { Name = "Big Sur Tea", Price = 25m },
        new() { Name = "Big Sur Tea", Price = 25m },
        new() { Name = "Jasmine Pearls", Price = 41m },
        new() { Name = "Dragonwell", Price = 30m },
        new() { Name = "White Peach Tea", Price = 29m },
        new() { Name = "Vanilla Berry Tea", Price = 21m },
        new() { Name = "Chaga Chai Mushroom Tea", Price = 20m },
        new() { Name = "Naked Pu-erh Tea", Price = 27m },
    };
}
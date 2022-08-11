using NeerCore.Data.EntityFramework.Abstractions;
using NeerCoreTestingSuite.WebApp.Data.Entities;

namespace NeerCore.Tests.Seeders;

public class AdditionalTeasSeeder : IEntityDataSeeder<Tea>
{
    public IEnumerable<Tea> Data => new[]
    {
        new Tea
        {
            Name = "Test Tea Ok 💁🏿",
            Price = 123.456m
        }
    };
}
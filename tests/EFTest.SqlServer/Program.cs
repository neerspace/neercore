using EFTest.SqlServer;
using EFTest.SqlServer.Entities;
using Microsoft.Extensions.DependencyInjection;
using NeerCore.Data.EntityFramework.Extensions;

var services = new ServiceCollection();
services.AddDatabase<TestDbContext>();

var provider = services.BuildServiceProvider();


var database = provider.GetRequiredService<TestDbContext>();
var animals = database.Set<Animal>().Where(e => e.Id != 2).ToList();
foreach (var animal in animals)
{
    Console.WriteLine("Animal: " + animal);
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeerCoreTestingSuite.WebApp.Data;
using NeerCoreTestingSuite.WebApp.Data.Entities;
using Xunit.Abstractions;
using static NeerCore.Tests.TestingEnvironment;

namespace NeerCore.Tests;

public class Test
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly SqliteDbContext _dbContext;

    public Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _dbContext = TestServices.GetRequiredService<SqliteDbContext>();
    }

    [Fact]
    public async Task Test_1()
    {
        _dbContext.Set<Tea>().Add(new Tea
        {
            Name = "Test",
            Price = 12
        });
        await _dbContext.SaveChangesAsync();

        var allTeas = await _dbContext.Set<Tea>().ToListAsync();
        foreach (Tea tea in allTeas)
        {
            _testOutputHelper.WriteLine(tea.Id + " – " + tea.Name.GetCurrentLocalization());
        }
    }
}
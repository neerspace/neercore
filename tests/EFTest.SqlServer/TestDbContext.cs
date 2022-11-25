using Microsoft.EntityFrameworkCore;
using NeerCore.Data.EntityFramework.Abstractions;

namespace EFTest.SqlServer;

public class TestDbContext : DbContext, IDatabaseContext
{
    
}
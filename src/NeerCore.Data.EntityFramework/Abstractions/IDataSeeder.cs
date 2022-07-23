using Microsoft.EntityFrameworkCore;

namespace NeerCore.Data.EntityFramework.Abstractions;

/// <summary>
///   Provides clean API to auto seed your data in DB.
/// </summary>
public interface IDataSeeder
{
    /// <summary>
    ///   Method that invokes to seed data.
    /// </summary>
    /// <param name="builder">An <see cref="ModelBuilder"/> instance.</param>
    void Seed(ModelBuilder builder);
}
namespace NeerCore.Data.EntityFramework.Design;

/// <summary>Model used to map connection string setting.</summary>
public record ConnectionStringOptions(IReadOnlyDictionary<string, string> ConnectionStrings);
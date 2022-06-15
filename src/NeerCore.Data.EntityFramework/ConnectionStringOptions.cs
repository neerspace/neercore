namespace NeerCore.Data.EntityFramework;

/// <summary>
///	Model used to map connection string setting.
/// </summary>
public record ConnectionStringOptions(IReadOnlyDictionary<string, string> ConnectionStrings);
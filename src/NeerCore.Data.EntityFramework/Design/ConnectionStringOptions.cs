namespace NeerCore.Data.EntityFramework.Design;

/// <summary>
///   Model used to map connection string setting.
/// </summary>
[Obsolete]
public record ConnectionStringOptions(IReadOnlyDictionary<string, string> ConnectionStrings);
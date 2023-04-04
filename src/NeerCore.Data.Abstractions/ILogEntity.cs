namespace NeerCore.Data.Abstractions;

/// <summary>
///
/// </summary>
public interface ILogEntity : ICreatableEntity
{
    /// <summary>
    ///
    /// </summary>
    string Level { get; }

    /// <summary>
    ///
    /// </summary>
    string Logger { get; }

    /// <summary>
    ///
    /// </summary>
    string Message { get; }

    /// <summary>
    ///
    /// </summary>
    string? Exception { get; }
}
namespace NeerCore.Api;

/// <summary>
///   Record that represents a default NeerCore HTTP error response.
/// </summary>
public sealed record Error
{
    /// <example>400</example>
    public int Status { get; init; }

    /// <example>ValidationFailed</example>
    public string Type { get; init; } = default!;

    /// <example>Something goes wrong :(</example>
    public string Message { get; init; } = default!;

    /// <summary>
    ///   A set of additional errors.
    /// </summary>
    /// <remarks>
    ///   Recommendation: pass a field name as dictionary key
    ///   and value should be plain <see cref="string"/> if field has only single message
    ///   or an array of <see cref="string"/>s if it has more then one error message.
    /// </remarks>
    public IReadOnlyDictionary<string, object>? Errors { get; init; }


    /// <inheritdoc cref="Error"/>
    public Error(int status, string type, string message, IReadOnlyDictionary<string, object>? errors = null)
    {
        Status = status;
        Type = type;
        Message = message;
        Errors = errors;
    }
}
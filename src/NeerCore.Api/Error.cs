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

    public IReadOnlyList<Details>? Errors { get; init; }


    public Error(int status, string type, string message, IReadOnlyList<Details>? errors = null)
    {
        Status = status;
        Type = type;
        Message = message;
        Errors = errors;
    }


    /// <summary>
    ///   Detailed information about single invalid field.
    /// </summary>
    public sealed record Details
    {
        /// <example>username</example>
        public string? Field { get; init; }

        /// <example>ValidationFailed</example>
        public string Message { get; init; } = default!;


        public Details(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}
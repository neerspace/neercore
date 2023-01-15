using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>400</b>.
///   <i>Bad request (or data not valid).</i>
/// </summary>
public sealed class ValidationFailedException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public override string ErrorType => "ValidationFailed";


    /// <summary>
    ///   Creates an instance of the <see cref="ValidationFailedException"/> with additional params.
    /// </summary>
    public ValidationFailedException(string message, IReadOnlyDictionary<string, object>? details = null) : base(message, details) { }

    /// <summary>
    ///   Creates an instance of the <see cref="ValidationFailedException"/>.
    ///   Useful when you need to throw this exception for single property.
    /// </summary>
    public ValidationFailedException(string field, string message) : base(field, message) { }
}
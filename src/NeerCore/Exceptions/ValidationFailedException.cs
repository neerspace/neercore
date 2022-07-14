using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>400</b>.
///   <i>Bad request (or data not valid).</i>
/// </summary>
public class ValidationFailedException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    public override string ErrorType => "ValidationFailed";


    public ValidationFailedException(string message, IReadOnlyList<ErrorDetails>? details = null) : base(message, details) { }
    public ValidationFailedException(string field, string message) : base(field, message) { }
}
using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>500</b>. <br/>
///   <i>Internal server error (or somethings goes wrong with your server).</i>
/// </summary>
public sealed class InternalServerException : HttpException
{
    /// <inheritdoc cref="HttpException.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    /// <inheritdoc cref="HttpException.ErrorType"/>
    public override string ErrorType => "InternalServerError";


    public InternalServerException(string message) : base(message) { }

    public InternalServerException(string message, Exception innerException) : base(message)
    {
        if (innerException is null)
            throw new ArgumentNullException(nameof(innerException));

        Details = new Dictionary<string, object>
        {
            { "exception", innerException.Message },
            { "trace", (innerException.StackTrace ?? innerException.ToString()).Split('\n').Select(x => x.Trim()) }
        };
    }
}
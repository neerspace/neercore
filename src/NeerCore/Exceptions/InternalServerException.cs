using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>500</b>. <br/>
///   <i>Internal server error (or somethings goes wrong with your server).</i>
/// </summary>
public sealed class InternalServerException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    public override string ErrorType => "InternalServerError";


    public InternalServerException(string message, bool addTraceDetails = false) : base(message)
    {
        if (addTraceDetails)
        {
            Details = new ErrorDetails[]
            {
                new("Trace", ToString())
            };
        }
    }
}
using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>422</b>.
///   Unprocessable entity <i>(indicates that the request was well-formed but was unable to be followed due to semantic errors).</i>
/// </summary>
public class UnprocessableEntityException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
    public override string ErrorType => "RequestEntityError";


    public UnprocessableEntityException(string message) : base(message) { }
}
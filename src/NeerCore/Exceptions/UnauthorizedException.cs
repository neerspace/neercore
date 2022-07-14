using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>401</b>.
///   Unauthorized <i>(or your resource requires an authorized user).</i>
/// </summary>
public class UnauthorizedException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    public override string ErrorType => "Unauthorized";


    public UnauthorizedException(string message) : base(message) { }
}
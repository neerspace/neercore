using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>403</b>.
///   Forbidden <i>(or user has no required permissions to access the resource).</i>
/// </summary>
public class ForbidException : HttpException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;
	public override string ErrorType => "AccessDenied";


	public ForbidException(string message) : base(message) { }
}
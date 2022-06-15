using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
/// Status Code: 401
/// </summary>
public class UnauthorizedException : HttpException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
	public override string ErrorType => "Unauthorized";


	public UnauthorizedException(string message) : base(message) { }
}
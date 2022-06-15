using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
/// Status Code: 500
/// </summary>
public class InternalServerException : HttpException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
	public override string ErrorType => "InternalServerError";


	public InternalServerException(string message) : base(message) { }
}
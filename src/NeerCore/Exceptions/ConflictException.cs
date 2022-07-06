using System.Net;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>409</b>.
///   Conflict <i>(indicates that the request could not be carried out because of a conflict on the server).</i>
/// </summary>
public class ConflictException : HttpException
{
	public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
	public override string ErrorType => "Conflict";


	public ConflictException(string message) : base(message) { }
}
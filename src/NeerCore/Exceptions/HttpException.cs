using System.Net;

namespace NeerCore.Exceptions;

public abstract class HttpException : Exception
{
	public abstract HttpStatusCode StatusCode { get; }
	public abstract string ErrorType { get; }
	public IReadOnlyList<ErrorDetails>? Details { get; set; }


	public HttpException(string message, IReadOnlyList<ErrorDetails>? details = null) : base(message) => Details = details;
	public HttpException(string field, string message) : base(message) => Details = new[] { new ErrorDetails(field, message) };
}
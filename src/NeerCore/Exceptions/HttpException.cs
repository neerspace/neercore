using System.Net;

namespace NeerCore.Exceptions;

/// <summary>The base exception that represents an HTTP error.</summary>
public abstract class HttpException : Exception
{
	/// <summary>
	///   The error response code with which it will be returned
	///   (if used within a web app, otherwise it will work like a normal exception).
	/// </summary>
	/// <remarks>
	///   1. I recommend using <b>4XX</b> for client errors and <b>5XX</b> for server errors.
	///   <br/>
	///   2. I strongly advise you not to use any other values for exceptions!
	/// </remarks>
	public abstract HttpStatusCode StatusCode { get; }

	/// <summary>Your custom error type.</summary>
	public abstract string ErrorType { get; }

	/// <summary>
	///   It is not enough for all exceptions to describe their type.
	///   It is often necessary to provide additional information,
	///   such as which fields have which list of errors.
	/// </summary>
	public IReadOnlyList<ErrorDetails>? Details { get; set; }


	protected HttpException(string message, IReadOnlyList<ErrorDetails>? details = null) : base(message)
	{
		Details = details;
	}

	protected HttpException(string field, string message) : base(message)
	{
		Details = new[] { new ErrorDetails(field, message) };
	}
}
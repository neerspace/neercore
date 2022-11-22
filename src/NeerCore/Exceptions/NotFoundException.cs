using System.Net;
using NeerCore.Extensions;

namespace NeerCore.Exceptions;

/// <summary>
///   The exception represents an HTTP error with status <b>404</b>.
///   <i>Not found (or resource not exists).</i>
/// </summary>
public class NotFoundException : HttpException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public override string ErrorType => "NotFound";


    public NotFoundException(string message) : base(message) { }
}

public sealed class NotFoundException<T> : NotFoundException
{
    public NotFoundException() : base($"Entity {typeof(T).Name} not found.") { }
}
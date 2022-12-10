namespace NeerCore.Api;

public sealed class ExceptionHandlerOptions
{
    public bool HandleHttpExceptions { get; set; } = true;
    public bool HandleFluentValidationExceptions { get; set; } = true;
    public bool Extended500ExceptionMessage { get; set; } = false;
}
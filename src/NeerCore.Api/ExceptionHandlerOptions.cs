namespace NeerCore.Api;

/// <summary>
///
/// </summary>
public sealed class ExceptionHandlerOptions
{
    /// <summary>
    ///
    /// </summary>
    public bool HandleHttpExceptions { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool HandleFluentValidationExceptions { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public bool Extended500ExceptionMessage { get; set; } = false;
}
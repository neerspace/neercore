namespace NeerCore.Data.Abstractions;

/// <summary>
///
/// </summary>
public interface IAspNetLogEntity : ILogEntity
{
    string? RequestUrl { get; }
    string? Ip { get; }
    string? UserAgent { get; }
}
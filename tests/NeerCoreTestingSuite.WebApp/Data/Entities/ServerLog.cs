using System.ComponentModel.DataAnnotations;
using NeerCore.Data.Abstractions;

namespace NeerCoreTestingSuite.WebApp.Data.Entities;

public class ServerLog : IAspNetLogEntity
{
    [Key]
    public DateTime Created { get; init; }

    public string Level { get; init; }
    public string Logger { get; init; }
    public string Message { get; init; }
    public string? Exception { get; init; }
    public string? RequestUrl { get; init; }
    public string? Ip { get; init; }
    public string? UserAgent { get; init; }
}
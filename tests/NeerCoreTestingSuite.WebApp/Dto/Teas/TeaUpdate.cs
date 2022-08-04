using System.Text.Json.Serialization;

namespace NeerCoreTestingSuite.WebApp.Dto.Teas;

public record TeaUpdate
{
    [JsonIgnore] public Guid Id { get; init; }

    /// <example>Black tea</example>
    public string Name { get; init; } = default!;
    // public LocalizedDictionary Name { get; init; } = default!;

    /// <example>19.50</example>
    public decimal Price { get; init; }
}
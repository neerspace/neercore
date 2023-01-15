using NeerCore.Localization;

namespace NeerCoreTestingSuite.WebApp.Dto.Teas;

/// <remarks>
/// Available filters: id, price, updated, created
/// </remarks>
public class Tea
{
    public Guid Id { get; init; }

    /// <example>Black tea</example>
    public LocalizedString Name { get; init; } = default!;

    /// <example>0.000663</example>
    public decimal PriceBTC { get; init; }

    /// <example>19.50</example>
    public decimal PriceUSD { get; init; }

    public DateTime? Updated { get; init; }

    public DateTime Created { get; init; }
}
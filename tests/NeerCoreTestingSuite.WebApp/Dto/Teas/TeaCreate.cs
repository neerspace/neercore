using FluentValidation;
using NeerCore.Data;

namespace NeerCoreTestingSuite.WebApp.Dto.Teas;

public record TeaCreate
{
    /// <example>Black tea</example>
    public LocalizedString Name { get; init; } = default!;

    /// <example>19.50</example>
    public decimal Price { get; init; }
}

public class TeaCreateValidator : AbstractValidator<TeaCreate>
{
    public TeaCreateValidator()
    {
        // RuleFor(o => o.Name).NotEmpty().Length(2, 64);
        RuleFor(o => o.Price).GreaterThan(0m);
    }
}
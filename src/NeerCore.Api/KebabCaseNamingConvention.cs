using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Api;

/// <summary>
///   Configures ASP.NET Core controllers to use kebab case naming by default.
/// </summary>
/// <remarks>kebab-case-example</remarks>
public sealed class KebabCaseNamingConvention : IOutboundParameterTransformer
{
    /// <summary>
    ///   Used to convert ControllerClassNames to kebab-case-style routes.
    /// </summary>
    public static void Use(MvcOptions options)
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseNamingConvention()));
    }

    private static readonly Regex KebabRegex = new("([a-z])([A-Z])");

    public string? TransformOutbound(object? value)
    {
        return value is null ? null : KebabRegex.Replace(value.ToString()!, "$1-$2").ToLower();
    }
}
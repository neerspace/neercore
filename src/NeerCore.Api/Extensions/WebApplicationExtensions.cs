using Microsoft.AspNetCore.Builder;

namespace NeerCore.Api.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    ///   Redirects requests from '<paramref name="from"/>' to '<paramref name="to"/>' URI.
    /// </summary>
    /// <param name="app">The web application used to configure the HTTP pipeline, and routes.</param>
    /// <param name="from">Source route.</param>
    /// <param name="to">Destination route.</param>
    public static void ForceRedirect(this WebApplication app, string from, string to)
    {
        app.MapGet(from, context =>
        {
            context.Response.Redirect(to, true);
            return Task.CompletedTask;
        });
    }
}
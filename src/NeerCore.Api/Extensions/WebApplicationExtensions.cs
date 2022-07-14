using Microsoft.AspNetCore.Builder;

namespace NeerCore.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ForceRedirect(this WebApplication app, string from, string to)
    {
        app.MapGet(from, context =>
        {
            context.Response.Redirect(to, true);
            return Task.CompletedTask;
        });
    }
}
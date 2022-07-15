using Microsoft.AspNetCore.Http;

namespace NeerCore.Api.Extensions;

public static class HttpResponseExtensions
{
    public static void SetNavigationHeaders(this HttpResponse response, int total, int page, int pageSize)
    {
        response.Headers["Navigation-Page"] = page.ToString();
        response.Headers["Navigation-Last-Page"] = ((int) Math.Ceiling((double) total / pageSize)).ToString();
        response.Headers["Count"] = total.ToString();
        response.Headers["Access-Control-Expose-Headers"] = "Navigation-Page,Navigation-Last-Page,Count";
    }
}
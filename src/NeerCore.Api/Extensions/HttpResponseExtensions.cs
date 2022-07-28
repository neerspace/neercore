using Microsoft.AspNetCore.Http;

namespace NeerCore.Api.Extensions;

public static class HttpResponseExtensions
{
    /// <summary>
    ///   Adds pagination info to the current response headers.
    ///   And also adds CORS header to allow the browser use pagination headers.
    /// </summary>
    /// <param name="response">Represents the outgoing side of an individual HTTP request.</param>
    /// <param name="total">Total count of all entities.</param>
    /// <param name="page">Current navigation page. (starts from 1)</param>
    /// <param name="pageSize">Count of items per each page.</param>
    public static void SetNavigationHeaders(this HttpResponse response, int total, int page, int pageSize)
    {
        response.Headers["Navigation-Page"] = page.ToString();
        response.Headers["Navigation-Last-Page"] = ((int)Math.Ceiling((double)total / pageSize)).ToString();
        response.Headers["Count"] = total.ToString();
        response.Headers["Access-Control-Expose-Headers"] = "Navigation-Page,Navigation-Last-Page,Count";
    }
}
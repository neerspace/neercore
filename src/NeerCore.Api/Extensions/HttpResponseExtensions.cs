using Microsoft.AspNetCore.Http;

namespace NeerCore.Api.Extensions;

public static class HttpResponseExtensions
{
    /// <summary>
    ///   Adds pagination info to the current response headers.
    ///   And also adds CORS header to allow the browser use pagination headers.
    ///   <br/>
    ///   Sets next headers:
    ///   <list type="bullet">
    ///     <item><b>Navigation-Page</b>: Current navigation page.</item>
    ///     <item><b>Navigation-Last-Page</b>: Max navigation page value.</item>
    ///     <item><b>Navigation-Total</b>: Totals count of filtered items (without pagination).</item>
    ///     <item><b>Access-Control-Expose-Headers</b>: Allows previous headers in CORS.</item>
    ///   </list>
    /// </summary>
    /// <remarks>For paging navigation style.</remarks>
    /// <example>
    ///   Controller action pseudo code:
    ///   <br/>
    ///   <code>
    ///     [HttpGet]
    ///     public IEnumerable&lt;Model&gt; GetList(string filters, int page = 1, int pageSize = 10)
    ///     {
    ///       IEnumerable&lt;Model&gt; models = _service.Filter(filters, page, pageSize);
    ///       int total = _service.Count(filters);
    ///       Response.SetNavigationHeaders(total, page, pageSize);
    ///       return models;
    ///     }
    ///   </code>
    /// </example>
    /// <param name="response">Represents the outgoing side of an individual HTTP request.</param>
    /// <param name="total">Total count of all entities.</param>
    /// <param name="page">Current navigation page. (starts from 1)</param>
    /// <param name="pageSize">Count of items per each page.</param>
    public static void SetNavigationHeaders(this HttpResponse response, int total, int page, int pageSize)
    {
        response.Headers["Navigation-Page"] = page.ToString();
        response.Headers["Navigation-Last-Page"] = ((int)Math.Ceiling((double)total / pageSize)).ToString();
        response.Headers["Navigation-Total"] = total.ToString();
        response.Headers["Access-Control-Expose-Headers"] = "Navigation-Page,Navigation-Last-Page,Navigation-Total";
    }

    /// <summary>
    ///   Adds pagination info to the current response headers.
    ///   And also adds CORS header to allow the browser use pagination headers.
    ///   <br/>
    ///   Sets next headers:
    ///   <list type="bullet">
    ///     <item><b>Navigation-Page</b>: Current navigation page.</item>
    ///     <item><b>Navigation-Last-Page</b>: Max navigation page value.</item>
    ///     <item><b>Navigation-Total</b>: Totals count of filtered items (without pagination).</item>
    ///     <item><b>Access-Control-Expose-Headers</b>: Allows previous headers in CORS.</item>
    ///   </list>
    /// </summary>
    /// <remarks>For skip-take navigation style.</remarks>
    /// <example>
    ///   Controller action pseudo code:
    ///   <br/>
    ///   <code>
    ///     [HttpGet]
    ///     public IEnumerable&lt;Model&gt; GetList(string filters, int skip = 0, int take = 10)
    ///     {
    ///       IEnumerable&lt;Model&gt; models = _service.Filter(filters, skip, take);
    ///       int total = _service.Count(filters);
    ///       Response.SetPaginationHeaders(total, skip, take);
    ///       return models;
    ///     }
    ///   </code>
    /// </example>
    /// <param name="response">Represents the outgoing side of an individual HTTP request.</param>
    /// <param name="total">Total count of all entities.</param>
    /// <param name="skip">Index of first element in the result sequence.</param>
    /// <param name="take">Count of items in result sequence.</param>
    public static void SetPaginationHeaders(this HttpResponse response, int total, int skip, int take)
    {
        response.Headers["Navigation-Page"] = ((int)Math.Ceiling((double)skip / take)).ToString();
        response.Headers["Navigation-Last-Page"] = ((int)Math.Ceiling((double)total / take)).ToString();
        response.Headers["Navigation-Total"] = total.ToString();
        response.Headers["Access-Control-Expose-Headers"] = "Navigation-Page,Navigation-Last-Page,Navigation-Total";
    }
}
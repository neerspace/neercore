using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace NeerCore.Api.ModelBinding;

/// <inheritdoc />
public sealed class FromBodyOrRouteModelBinderProvider : IModelBinderProvider
{
    private readonly IList<IInputFormatter> _formatters;
    private readonly IHttpRequestStreamReaderFactory _readerFactory;
    private readonly ILoggerFactory? _loggerFactory;
    private readonly MvcOptions? _options;

    /// <summary>
    /// Creates a new <see cref="FromBodyOrRouteModelBinderProvider"/>.
    /// </summary>
    /// <param name="formatters">The list of <see cref="IInputFormatter"/>.</param>
    /// <param name="readerFactory">The <see cref="IHttpRequestStreamReaderFactory"/>.</param>
    public FromBodyOrRouteModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        : this(formatters, readerFactory, loggerFactory: NullLoggerFactory.Instance) { }

    /// <summary>
    /// Creates a new <see cref="FromBodyOrRouteModelBinderProvider"/>.
    /// </summary>
    /// <param name="formatters">The list of <see cref="IInputFormatter"/>.</param>
    /// <param name="readerFactory">The <see cref="IHttpRequestStreamReaderFactory"/>.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    /// <param name="options">The <see cref="MvcOptions"/>.</param>
    public FromBodyOrRouteModelBinderProvider(
        IList<IInputFormatter> formatters,
        IHttpRequestStreamReaderFactory readerFactory,
        ILoggerFactory? loggerFactory,
        MvcOptions? options = null)
    {
        _formatters = formatters ?? throw new ArgumentNullException(nameof(formatters));
        _readerFactory = readerFactory ?? throw new ArgumentNullException(nameof(readerFactory));
        _loggerFactory = loggerFactory;
        _options = options;
    }

    /// <inheritdoc />
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.BindingInfo.BindingSource == BindingSource.Body
            ? new BodyOrRouteModelBinder(_formatters, _readerFactory, _loggerFactory, _options)
            : null;
    }
}
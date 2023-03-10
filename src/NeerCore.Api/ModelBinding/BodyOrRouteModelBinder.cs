using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace NeerCore.Api.ModelBinding;

/// <summary>
/// An <see cref="IModelBinder"/> which binds models from the request body using an <see cref="IInputFormatter"/>
/// when a model has the binding source <see cref="BindingSource.Body"/>.
/// and then overrides properties from the route
/// when a property has the binding source <see cref="BindingSource.Path"/>.
/// </summary>
public class BodyOrRouteModelBinder : BodyModelBinder, IModelBinder
{
    /// <summary>
    /// Creates a new <see cref="BodyOrRouteModelBinder"/>.
    /// </summary>
    /// <param name="formatters">The list of <see cref="IInputFormatter"/>.</param>
    /// <param name="readerFactory">
    /// The <see cref="IHttpRequestStreamReaderFactory"/>, used to create <see cref="System.IO.TextReader"/>
    /// instances for reading the request body.
    /// </param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
    /// <param name="options">The <see cref="MvcOptions"/>.</param>
    public BodyOrRouteModelBinder(
        IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory,
        ILoggerFactory? loggerFactory = null, MvcOptions? options = null)
        : base(formatters, readerFactory, loggerFactory, options) { }


    /// <inheritdoc />
    public new async Task BindModelAsync(ModelBindingContext context)
    {
        await base.BindModelAsync(context);
        var bodyModel = context.Result.Model;

        var routeProperties = context.ModelMetadata.Properties
            .Where(p => p.BindingSource == BindingSource.Path).ToArray();
        var modelType = context.ModelMetadata.UnderlyingOrModelType;

        foreach (var propMetadata in routeProperties)
        {
            if (propMetadata.BinderModelName is null && propMetadata.PropertyName is null)
                continue;
            var routeKey = propMetadata.BinderModelName ?? propMetadata.PropertyName!;
            var routeValue = context.ActionContext.RouteData.Values[routeKey];
            var targetProp = modelType.GetProperty(propMetadata.PropertyName!);
            if (targetProp is null)
                throw new InvalidOperationException(
                    $"Property with name '{propMetadata.PropertyName}' does not exist in '{modelType.FullName}'");

            try
            {
                var finalType = Convert.ChangeType(routeValue, propMetadata.UnderlyingOrModelType);
                targetProp.SetValue(bodyModel, finalType);
            }
            catch (Exception)
            {
                context.ModelState.AddModelError(routeKey, $"The value '{routeValue}' is not valid.");
            }
        }
    }
}
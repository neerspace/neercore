using Microsoft.AspNetCore.Mvc.ModelBinding;
using NeerCore.Data.EntityFramework.Typeids;

namespace NeerCore.Api.Typeids;

public class TypeidsModelBinder : IModelBinder
{
    private static ITypeidsProcessor? _idsProcessor;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        string? sourceValue = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;
        var idsProcessor = _idsProcessor ??= bindingContext.HttpContext.RequestServices.GetRequiredService<ITypeidsProcessor>();

        object? identifier = idsProcessor.DeserializeIdentifier(sourceValue, bindingContext.ModelType);
        bindingContext.Result = ModelBindingResult.Success(identifier);

        return Task.CompletedTask;
    }
}
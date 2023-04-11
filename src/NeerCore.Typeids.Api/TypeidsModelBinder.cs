using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace NeerCore.Typeids.Api;

public class TypeidsModelBinder : IModelBinder
{
    private static ITypeidsProcessor? idsProcessor;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        string? sourceValue = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;
        idsProcessor ??= bindingContext.HttpContext.RequestServices.GetRequiredService<ITypeidsProcessor>();

        object? identifier = idsProcessor.DeserializeIdentifier(sourceValue, bindingContext.ModelType);
        bindingContext.Result = ModelBindingResult.Success(identifier);

        return Task.CompletedTask;
    }
}
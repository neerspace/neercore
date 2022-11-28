using Microsoft.AspNetCore.Mvc.ModelBinding;
using NeerCore.DependencyInjection.Extensions;
using NeerCore.Typeids;
using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids.Api;

public class TypeidsModelBinderProvider : IModelBinderProvider
{
    private static readonly Type IdentifierTypeBase = typeof(ITypeIdentifier<>);

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (context.Metadata.ModelType.InheritsFrom(IdentifierTypeBase))
            return new TypeidsModelBinder();

        return null;
    }
}
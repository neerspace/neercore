using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Data.EntityFramework.Typeids.Abstractions;

namespace NeerCore.Data.EntityFramework.Typeids.ValueConverters;

public class TypeidsValueConverter<TIdentifier, TValue> : ValueConverter<TIdentifier?, TValue>
    where TIdentifier : struct, ITypeIdentifier<TValue>
    where TValue : struct
{
    public TypeidsValueConverter(ConverterMappingHints? mappingHints = null) : base(
        id => id.HasValue ? id.Value.Value : default,
        value => IdentifierFactory<TIdentifier, TValue>.Create(value),
        mappingHints
    ) { }
}
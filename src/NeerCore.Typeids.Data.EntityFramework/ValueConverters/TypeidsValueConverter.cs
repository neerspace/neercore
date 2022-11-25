using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeerCore.Typeids.Data.EntityFramework.Abstractions;

namespace NeerCore.Typeids.Data.EntityFramework.ValueConverters;

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
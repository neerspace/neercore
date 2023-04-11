using NeerCore.Typeids.Abstractions;

namespace EFTest.SqlServer;

public readonly record struct AnimalId(int Value) : IIntIdentifier
{
    public static implicit operator int(AnimalId id) => id.Value;
    public static implicit operator AnimalId(int value) => new(value);
}

public readonly record struct OtherNotId(long Value) : ILongIntIdentifier;
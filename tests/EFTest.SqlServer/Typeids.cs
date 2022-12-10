using NeerCore.Typeids.Abstractions;

namespace EFTest.SqlServer;

public partial record struct AnimalId(int Value)
{
    public static implicit operator int(AnimalId id) => id.Value;
    public static implicit operator AnimalId(int value) => new(value);
}

public partial record struct AnimalId : IIntIdentifier { }
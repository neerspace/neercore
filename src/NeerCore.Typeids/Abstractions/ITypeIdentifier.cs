namespace NeerCore.Typeids.Abstractions;

public interface ITypeIdentifier<out TValue>
{
    TValue Value { get; }
}

public interface IGuidIdentifier : ITypeIdentifier<Guid> { }

public interface IStringIdentifier : ITypeIdentifier<string> { }

public interface IShortIntIdentifier : ITypeIdentifier<short> { }

public interface IIntIdentifier : ITypeIdentifier<int> { }

public interface ILongIntIdentifier : ITypeIdentifier<long> { }
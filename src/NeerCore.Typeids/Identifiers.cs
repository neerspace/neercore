using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids;

public record struct IntIdentifier(int Value) : IIntIdentifier;
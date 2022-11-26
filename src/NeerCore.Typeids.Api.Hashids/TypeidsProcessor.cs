using HashidsNet;
using NeerCore.Typeids.Data.EntityFramework;
using NeerCore.Typeids.Data.EntityFramework.Abstractions;
using NeerCore.DependencyInjection;

namespace NeerCore.Typeids.Api;

[Service(Lifetime = Lifetime.Singleton)]
public class TypeidsProcessor : ITypeidsProcessor
{
    private readonly IHashids _hashids;

    public TypeidsProcessor(IHashids hashids)
    {
        _hashids = hashids;
    }

    public string SerializeString<TIdentifier, TValue>(TIdentifier identifier)
        where TIdentifier : ITypeIdentifier<TValue> where TValue : new() =>
        SerializeInternal(identifier);

    public string SerializeString(object? identifier) =>
        SerializeInternal(identifier);

    public TIdentifier DeserializeIdentifier<TIdentifier, TValue>(string? stringValue)
        where TIdentifier : ITypeIdentifier<TValue> where TValue : new() =>
        (TIdentifier)DeserializeIdentifier(stringValue, typeof(TIdentifier));

    public object DeserializeIdentifier(string? stringValue, Type targetIdentifier)
    {
        stringValue = stringValue?.ToUpper();

        if (targetIdentifier == AchievementId)
            return new AchievementId((short)FirstOrError(_hashids.Decode(stringValue)));
        if (targetIdentifier == ChatId)
            return new ChatId(FirstOrError(_hashids.DecodeLong(stringValue)));
        if (targetIdentifier == IdentityId)
            return new IdentityId(FirstOrError(_hashids.DecodeLong(stringValue)));
        if (targetIdentifier == PublicationId)
            return new PublicationId(FirstOrError(_hashids.Decode(stringValue)));
        if (targetIdentifier == ResourceId && !string.IsNullOrEmpty(stringValue))
            return new ResourceId(new Guid(stringValue));
        if (targetIdentifier == WorldId)
            return new WorldId(FirstOrError(_hashids.DecodeLong(stringValue)));

        throw new NotSupportedException($"Deserialization for identifier '{targetIdentifier}' does not supported.");
    }

    private string SerializeInternal<T>(T? identifier)
    {
        return identifier switch
        {
            AchievementId achievementId => _hashids.Encode(achievementId.Value),
            ChatId chatId               => _hashids.EncodeLong(chatId.Value),
            IdentityId identityId       => _hashids.EncodeLong(identityId.Value),
            PublicationId publicationId => _hashids.Encode(publicationId.Value),
            ResourceId resourceId       => resourceId.ToString(),
            WorldId worldId             => _hashids.EncodeLong(worldId.Value),
            _                           => throw new NotSupportedException($"Serialization for identifier '{identifier?.GetType()}' does not supported.")
        };
    }

    private static T FirstOrError<T>(IList<T> collection) => collection is { Count: > 0 } ? collection[0] : throw new Exception("Invalid hash.");
}
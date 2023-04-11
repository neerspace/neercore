using System.Reflection;
using System.Reflection.Emit;
using NeerCore.Typeids.Abstractions;

namespace NeerCore.Typeids.Internal;

internal static class IdentifierFactory<TIdentifier, TValue>
    where TIdentifier : ITypeIdentifier<TValue>
    where TValue : new()
{
    private static Func<TValue, TIdentifier>? identifierConstructor;
    private static Func<TValue, TIdentifier> IdentifierConstructor => identifierConstructor ??= GenerateConstructorInvoker();

    public static TIdentifier CreateUnsafe<T>(T value)
    {
        if (value is TValue paramValue)
            return IdentifierConstructor(paramValue);

        throw new InvalidCastException($"Invalid value type of '{typeof(T).Name}' for identifier of type '{typeof(TIdentifier).Name}'");
    }

    public static TIdentifier Create(TValue value) => IdentifierConstructor(value);


    private static Func<TValue, TIdentifier> GenerateConstructorInvoker()
    {
        var type = typeof(TIdentifier);
        var ctorParamTypes = new[] { typeof(TValue) };

        var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, ctorParamTypes)!;
        var ctorMethod = new DynamicMethod(type.Name + "_ctor", type, ctorParamTypes, type.Module, true);

        var ilGenerator = ctorMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Call, ctor);
        ilGenerator.Emit(OpCodes.Ret);
        var constructorInvoker = (Func<TValue, TIdentifier>)ctorMethod.CreateDelegate(typeof(Func<TValue, TIdentifier>));
        return constructorInvoker;
    }
}
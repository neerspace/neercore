using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace NeerCore.Typeids.Internal;

internal static class IdentifierFactory
{
    private delegate object GeneralConstructor(object value);

    private static readonly ConcurrentDictionary<Type, GeneralConstructor> TypeConstructors = new();


    public static object Create(object value, Type type) => GetOrCreateConstructor(type).Invoke(value);

    private static GeneralConstructor GetOrCreateConstructor(Type type) => TypeConstructors.GetOrAdd(type, GenerateConstructorInvoker);


    private static GeneralConstructor GenerateConstructorInvoker(Type type)
    {
        var ctorParamTypes = new[] { type.GetIdentifierValueType() };

        var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, ctorParamTypes)!;
        var ctorMethod = new DynamicMethod(type.Name + "_ctor", type, ctorParamTypes, type.Module, true);

        var ilGenerator = ctorMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Call, ctor);
        ilGenerator.Emit(OpCodes.Ret);
        var constructorInvoker = (GeneralConstructor) ctorMethod.CreateDelegate(typeof(GeneralConstructor));
        return constructorInvoker;
    }
}
using System.Reflection;

namespace NeerCore.Typeids.Data.EntityFramework.Options;

public class TypeidsOptions
{
    /// <summary>
    ///   If <b>true</b> <c>ValueGeneratedOnAdd</c>
    ///   will be called for every numeric type identifier.
    /// </summary>
    public bool NumericTypesAutoincrementByDefault { get; set; } = true;

    /// <summary>
    ///   An array of assemblies where typeids classes located
    ///   (by default a calling assembly will be used).
    /// </summary>
    public Assembly[]? Assemblies { get; set; }
}
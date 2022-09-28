using System.ComponentModel.DataAnnotations.Schema;

namespace NeerCore.Data.EntityFramework.Attributes;

/// <summary>
///   Extension for <see cref="ColumnAttribute"/> to determinate decimal type accuracy.
/// </summary>
public sealed class DecimalColumnAttribute : ColumnAttribute
{
    public DecimalColumnAttribute(int integerAccuracy = 18, int fractionalAccuracy = 8)
    {
        TypeName = $"decimal({integerAccuracy},{fractionalAccuracy})";
    }
}
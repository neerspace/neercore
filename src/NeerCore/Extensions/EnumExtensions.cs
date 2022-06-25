using System.Reflection;
using System.Runtime.Serialization;

namespace NeerCore.Extensions;

public static class EnumExtensions
{
	/// <summary>
	/// Converts enum to string by <see cref="EnumMemberAttribute"/> attribute value.
	///	<br/><br/>
	/// Use this method with care, because if the attribute is not specified, an error will be thrown.
	/// If you are sure that everything is OK, then you can use it, and if not, then it is better to
	/// look at the safe <see cref="GetDisplayName{TEnum}"/> method.
	/// </summary>
	/// <param name="value">Enum member (for example <see cref="DayOfWeek"/>.<see cref="DayOfWeek.Sunday"/>)</param>
	/// <returns>String value for given enum member</returns>
	public static string GetRequiredDisplayName<TEnum>(this TEnum value)
			where TEnum : Enum
	{
		return typeof(TEnum)
				.GetMember(value.ToString()).Single()
				.GetCustomAttribute<EnumMemberAttribute>()!.Value!;
	}

	/// <summary>
	/// Converts enum to string by <see cref="EnumMemberAttribute"/> attribute value
	/// or returns default ToString() if attribute is not provided.
	/// </summary>
	/// <param name="value">Enum member (for example <see cref="DayOfWeek"/>.<see cref="DayOfWeek.Sunday"/>)</param>
	/// <returns>String value for given enum member</returns>
	public static string GetDisplayName<TEnum>(this TEnum value)
			where TEnum : Enum
	{
		return value.GetType()
				.GetMember(value.ToString()).Single()
				.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
	}
}
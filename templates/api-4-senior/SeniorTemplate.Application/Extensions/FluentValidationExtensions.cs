using FluentValidation;

namespace SeniorTemplate.Application.Extensions;

public static class FluentValidationExtensions
{
	public static IRuleBuilder<T, string?> UserName<T>(this IRuleBuilder<T, string?> ruleBuilder)
	{
		return ruleBuilder.Length(2, 64);
	}

	public static IRuleBuilder<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
	{
		return ruleBuilder.Matches(@"\+?[0-9]{6,20}");
	}
}
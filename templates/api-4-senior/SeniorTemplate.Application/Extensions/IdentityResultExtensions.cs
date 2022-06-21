using System.Globalization;
using Microsoft.AspNetCore.Identity;
using NeerCore;

namespace SeniorTemplate.Application.Extensions;

public static class IdentityResultExtensions
{
	private static readonly TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
	private static readonly string[] fields = { "email", "username", "password" };

	public static IReadOnlyList<ErrorDetails> ToErrorDetails(this IdentityResult identityResult)
	{
		return identityResult.Errors.Select(err =>
				new ErrorDetails(FieldName(err), err.Description)).ToArray();
	}

	private static string FieldName(IdentityError err)
	{
		string? fieldName = fields.FirstOrDefault(f => err.Code.ToLower().Contains(f));
		return textInfo.ToTitleCase(fieldName ?? err.Code);
	}
}
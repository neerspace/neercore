using NeerCore.Extensions;

namespace NeerCore.UnitTests;

public class StringExtensionsTests
{
	[Fact]
	public void CamelCaseToWords_When_ValueIsValid()
	{
		string value = "CamelCase";
		string expected = "Camel Case";

		string actual = value.CamelCaseToWords();

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void CamelCaseToWords_When_ValueIsNullOrEmpty()
	{
		string emptyValue = string.Empty;
		string nullValue = string.Empty;

		Assert.Throws<ArgumentNullException>(() => emptyValue.CamelCaseToWords());
		Assert.Throws<ArgumentNullException>(() => nullValue.CamelCaseToWords());
	}
}
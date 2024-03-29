namespace NeerCore.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("CamelCase", "Camel case")]
    [InlineData(null, null)]
    [InlineData("", null)]
    public void Test_CamelCaseToWords(string value, string? expected)
    {
        // if (string.IsNullOrEmpty(value))
        // {
        //     Assert.Throws<ArgumentNullException>(value.CamelCaseToWords);
        // }
        // else
        // {
        //     string actual = value.CamelCaseToWords();
        //     Assert.Equal(expected, actual);
        // }
    }

    [Theory]
    [InlineData("CamelCase", "camel_case")]
    [InlineData(null, null)]
    [InlineData("", null)]
    public void Test_ToSnakeCase(string value, string? expected)
    {
        if (string.IsNullOrEmpty(value))
        {
            Assert.Throws<ArgumentNullException>(value.ToSnakeCase);
        }
        else
        {
            string actual = value.ToSnakeCase();
            Assert.Equal(expected, actual);
        }
    }
}
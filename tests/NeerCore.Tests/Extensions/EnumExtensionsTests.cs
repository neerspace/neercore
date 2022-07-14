using System.Runtime.Serialization;

namespace NeerCore.Tests.Extensions;

public class EnumExtensionsTests
{
    public enum TestEnum
    {
        [EnumMember(Value = "AttributeValue")] WithEnumMemberAttribute,
        PlainEnumMember,
    }

    [Theory]
    [InlineData(TestEnum.WithEnumMemberAttribute, "AttributeValue")]
    [InlineData(TestEnum.PlainEnumMember, null)]
    public void Test_GetRequiredDisplayName(TestEnum value, string? expected)
    {
        if (expected is null)
        {
            Assert.Throws<NullReferenceException>(() => value.GetRequiredDisplayName());
        }
        else
        {
            string actual = value.GetRequiredDisplayName();
            Assert.Equal(expected, actual);
        }
    }

    [Theory]
    [InlineData(TestEnum.WithEnumMemberAttribute, "AttributeValue")]
    [InlineData(TestEnum.PlainEnumMember, nameof(TestEnum.PlainEnumMember))]
    public void Test_GetDisplayName(TestEnum value, string expected)
    {
        string actual = value.GetDisplayName();
        Assert.Equal(expected, actual);
    }
}
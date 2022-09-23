namespace NeerCore.Tests.Extensions;

public class EnumerableExtensionsTests
{
    [Theory]
    [InlineData(new object[] { "item_1", "item_2" }, true)]
    [InlineData(new object[] { "item_1" }, true)]
    [InlineData(new object[0], false)]
    public void Test_FirstOr404(object[] source, bool expected)
    {
        if (expected)
        {
            object actual = source.FirstOr404();
            Assert.Equal(source[0], actual);
        }
        else
        {
            Assert.Throws<NotFoundException>(() => source.FirstOr404());
        }
    }

    [Theory]
    [InlineData("item_1", "item_2")]
    public void Test_FirstOr404_With_Predicate(params object[] source)
    {
        object actual = source.FirstOr404(x => (string)x == "item_2");
        Assert.Equal("item_2", actual);
    }
}
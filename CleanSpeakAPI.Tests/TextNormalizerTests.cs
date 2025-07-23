using CleanSpeakAPI.Utils;

namespace CleanSpeakAPI.Tests;

public class TextNormalizerTests
{
    [Theory]
    [InlineData("Zażółć gęślą jaźń! ", "zazolc gesla jazn")]
    [InlineData("Hello World!", "hello world")]
    [InlineData("1234 ABC!", "1234 abc")]
    [InlineData("", "")]
    [InlineData("     ", "")]
    public void Normalize_RemovesSpecialCharactersAndLowercases(string input, string expected)
    {
        var result = TextNormalizer.Normalize(input);
        Assert.Equal(expected, result);
    }
}

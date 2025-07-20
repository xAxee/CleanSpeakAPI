using CleanSpeakAPI.Services;
using Xunit;

namespace CleanSpeakAPI.Tests;

public class TextModerationServiceTests
{
    [Fact]
    public void Analyze_ReturnsPrediction_WithValidText()
    {
        var service = new TextModerationService();

        var response = service.Analyze("Miłego dnia!");

        Assert.NotNull(response);
        Assert.NotEmpty(response.Category);
        Assert.True(response.Probabilities.ContainsKey("friendly"));
    }
}

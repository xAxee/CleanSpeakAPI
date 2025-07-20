namespace CleanSpeakAPI.Models;

public class ModerationPredictionResponse
{
    public required string Category { get; set; }
    public required Dictionary<string, float> Probabilities { get; set; }

    public static ModerationPredictionResponse FromPrediction(ModerationResult result, string[] labelOrder)
    {
        if (labelOrder == null)
            throw new ArgumentNullException(nameof(labelOrder), "labelOrder is null");

        if (result?.Probabilities == null)
            throw new ArgumentNullException(nameof(result.Probabilities), "Probabilities is null");

        var probabilities = labelOrder
            .Select((label, index) => new { label, score = result.Probabilities.ElementAtOrDefault(index) })
            .ToDictionary(x => x.label, x => x.score);

        var maxIndex = result.Probabilities
        .Select((value, index) => new { value, index })
        .OrderByDescending(x => x.value)
        .First().index;

        var predictedCategory = labelOrder.ElementAtOrDefault(maxIndex) ?? "unknown";

        return new ModerationPredictionResponse
        {
            Category = predictedCategory,
            Probabilities = probabilities
        };
    }

}

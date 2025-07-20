using Microsoft.ML.Data;

namespace CleanSpeakAPI.Models;

public class ModerationResult
{
    [ColumnName("PredictedLabel")]
    public string Category { get; set; } = default!;

    [ColumnName("Score")]
    public float[] Probabilities { get; set; } = default!;
}

using Microsoft.ML.Data;

public class ProfanityData
{
    [LoadColumn(0)]
    public string Text { get; set; }

    [LoadColumn(1)]
    public bool Label { get; set; }
}

public class ProfanityPrediction
{
    [ColumnName("PredictedLabel")]
    public bool IsProfane { get; set; }
}

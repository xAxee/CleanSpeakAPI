using Microsoft.ML;
using Microsoft.ML.Transforms.Text;

public class ProfanityDetector
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private PredictionEngine<ProfanityData, ProfanityPrediction> _predictionEngine;
    private readonly string _modelPath = "Data/model.zip";
    private readonly string _keywordsPath = "Data/keywords.txt";
    private readonly string _dataPath = "Data/data.csv";
    private HashSet<string> _keywords;

    public ProfanityDetector()
    {
        _mlContext = new MLContext();
        LoadKeywords();

        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out _);
        }
        else
        {
            TrainModel();
        }

        _predictionEngine = _mlContext.Model.CreatePredictionEngine<ProfanityData, ProfanityPrediction>(_model);
    }

    private void LoadKeywords()
    {
        if (File.Exists(_keywordsPath))
        {
            _keywords = new HashSet<string>(
                File.ReadAllLines(_keywordsPath)
                    .Select(word => TextPreprocessing.NormalizeText(word))
            );
        }
        else
        {
            _keywords = new HashSet<string>();
        }
    }

    public void TrainModel()
    {
        var data = _mlContext.Data.LoadFromTextFile<ProfanityData>(_dataPath, separatorChar: ';', hasHeader: true);

        var transformedData = _mlContext.Data.CreateEnumerable<ProfanityData>(data, reuseRowObject: false)
            .Select(row => new ProfanityData
            {
                Text = TextPreprocessing.NormalizeText(row.Text),
                Label = row.Label
            })
            .DistinctBy(x => x.Text)
            .ToList();

        var dataView = _mlContext.Data.LoadFromEnumerable(transformedData);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", new TextFeaturizingEstimator.Options
        {
            WordFeatureExtractor = new WordBagEstimator.Options { NgramLength = 2, UseAllLengths = true },
        }, "Text")
        .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"))
        .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
            labelColumnName: "Label", featureColumnName: "Features"))
        .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        var model = pipeline.Fit(dataView);
        _mlContext.Model.Save(model, dataView.Schema, _modelPath);
    }

    public (string normalizeText, bool IsProfane, List<string> DetectedProfanities) Predict(string text)
    {
        text = TextPreprocessing.NormalizeText(text);

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var detectedProfanities = words.Where(word => _keywords.Contains(word)).ToList();
        bool isProfane = detectedProfanities.Any();

        if (words.Length > 10 && !isProfane)
        {
            for (int i = 0; i < words.Length; i += 10)
            {
                var subText = string.Join(" ", words.Skip(i).Take(10));
                isProfane = _predictionEngine.Predict(new ProfanityData { Text = subText }).IsProfane;
            }
        }
        else
        {
            isProfane = _predictionEngine.Predict(new ProfanityData { Text = text }).IsProfane;
        }

        return (text, isProfane, detectedProfanities.ToList());
    }

    private (bool IsProfane, List<string> DetectedProfanities) CheckSubText(string subText)
    {
        var words = subText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var detectedProfanities = words.Where(word => _keywords.Contains(word)).ToList();
        bool isProfane = detectedProfanities.Any();
        return (isProfane, detectedProfanities);
    }
}
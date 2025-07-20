using CleanSpeakAPI.Models;
using CleanSpeakAPI.Utils;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CleanSpeakAPI.Services;

public class TextModerationService
{
    private readonly MLContext _mlContext;
    private ITransformer _model = default!;
    private PredictionEngine<ModerationInput, ModerationResult> _engine = default!;
    private readonly string _modelPath = "Data/model.zip";
    private readonly string _dataPath = "Data/data.csv";
    private string[] _labelOrder = default!;

    public TextModerationService()
    {
        _mlContext = new MLContext();
        LoadOrTrainModel();
    }

    private void LoadOrTrainModel()
    {
        if (File.Exists(_modelPath))
        {
            _model = _mlContext.Model.Load(_modelPath, out _);

            var data = _mlContext.Data.LoadFromTextFile<ModerationInput>(_dataPath, separatorChar: ';', hasHeader: false);

            var preprocessed = _mlContext.Data.CreateEnumerable<ModerationInput>(data, reuseRowObject: false)
                .Select(d => new ModerationInput
                {
                    Text = TextNormalizer.Normalize(d.Text),
                    Label = d.Label
                })
                .DistinctBy(x => x.Text)
                .ToList();

            var view = _mlContext.Data.LoadFromEnumerable(preprocessed);
            var labelPipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Label");
            var labelTransformed = labelPipeline.Fit(view).Transform(view);

            LoadLabelOrder(labelTransformed);
        }
        else
        {
            TrainModel();
        }
        _engine = _mlContext.Model.CreatePredictionEngine<ModerationInput, ModerationResult>(_model);
    }

    public void TrainModel()
    {
        var data = _mlContext.Data.LoadFromTextFile<ModerationInput>(_dataPath, separatorChar: ';', hasHeader: false);

        var preprocessed = _mlContext.Data.CreateEnumerable<ModerationInput>(data, reuseRowObject: false)
            .Select(d => new ModerationInput
            {
                Text = TextNormalizer.Normalize(d.Text),
                Label = d.Label
            })
            .DistinctBy(x => x.Text)
            .ToList();

        var view = _mlContext.Data.LoadFromEnumerable(preprocessed);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(ModerationInput.Text))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"))
            .Append(_mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy())
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        _model = pipeline.Fit(view);

        var labelPipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Label");
        var labelTransformed = labelPipeline.Fit(view).Transform(view);

        LoadLabelOrder(labelTransformed);

        _mlContext.Model.Save(_model, view.Schema, _modelPath);
    }


    public ModerationPredictionResponse Analyze(string input)
    {
        var normalized = TextNormalizer.Normalize(input);
        var prediction = _engine.Predict(new ModerationInput { Text = normalized });
        return ModerationPredictionResponse.FromPrediction(prediction, _labelOrder);
    }

    private void LoadLabelOrder(IDataView transformedView)
    {
        var labelBuffer = default(VBuffer<ReadOnlyMemory<char>>);
        transformedView.Schema["Label"].GetKeyValues(ref labelBuffer);

        _labelOrder = labelBuffer.DenseValues()
            .Select(x => x.ToString())
            .ToArray();
    }

}
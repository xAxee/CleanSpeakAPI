using Microsoft.ML.Data;
using System.Text.Json.Serialization;

namespace CleanSpeakAPI.Models;

public class ModerationInput
{
    [LoadColumn(0)]
    public string Text { get; set; } = default!;
    [JsonIgnore]

    [LoadColumn(1)]
    public string Label { get; set; } = string.Empty;
}
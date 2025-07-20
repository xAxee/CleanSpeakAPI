using System.Globalization;
using System.Text;

namespace CleanSpeakAPI.Utils;

public static class TextNormalizer
{
    private static readonly Dictionary<char, char> CustomMap = new()
    {
        ['ł'] = 'l',
        ['Ł'] = 'l'
    };
    public static string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var normalized = text.ToLower().Trim().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var cleanChar = CustomMap.ContainsKey(c) ? CustomMap[c] : c;
            var category = CharUnicodeInfo.GetUnicodeCategory(cleanChar);
            if (category != UnicodeCategory.NonSpacingMark && (char.IsLetter(cleanChar) || char.IsDigit(cleanChar) || cleanChar == ' '))
            {
                sb.Append(cleanChar);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
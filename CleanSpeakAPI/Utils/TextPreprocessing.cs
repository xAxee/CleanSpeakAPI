using System.Globalization;
using System.Text;

public static class TextPreprocessing
{
    public static string NormalizeText(string text)
    {
        var normalized = text.ToLower().Trim().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && char.IsLetter(c) || c == ' ' || char.IsDigit(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}

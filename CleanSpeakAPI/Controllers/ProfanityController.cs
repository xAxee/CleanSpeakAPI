using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProfanityController : ControllerBase
{
    private readonly ProfanityDetector _detector;

    public ProfanityController(ProfanityDetector detector)
    {
        _detector = detector;
    }

    [HttpPost("check")]
    public IActionResult CheckProfanity([FromBody] string text)
    {
        var (normalizeText, isProfane, detectedProfanities) = _detector.Predict(text);
        return Ok(new { Text = normalizeText, IsProfane = isProfane, DetectedProfanities = detectedProfanities });
    }

    [HttpPost("checkList")]
    public IActionResult CheckProfanityList([FromBody] string[] texts)
    {
        var results = texts.Select(text =>
        {
            var (normalizeText, isProfane, detectedProfanities) = _detector.Predict(text);
            return new { Text = normalizeText, IsProfane = isProfane, DetectedProfanities = detectedProfanities };
        });
        return Ok(results);
    }
}
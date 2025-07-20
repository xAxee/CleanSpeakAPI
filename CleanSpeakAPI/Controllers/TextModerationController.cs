using CleanSpeakAPI.Models;
using CleanSpeakAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static TorchSharp.torch.nn;

namespace CleanSpeakAPI.Controllers;

/// <summary>
/// API do klasyfikacji tekstu (moderacja treści).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TextModerationController : ControllerBase
{
    private readonly TextModerationService _moderationService;

    public TextModerationController(TextModerationService moderationService)
    {
        _moderationService = moderationService;
    }

    /// <summary>
    /// Klasyfikuje przesłany tekst jako „vulgar”, „friendly”, „neutral” itp.
    /// </summary>
    /// <param name="request">Tekst do analizy.</param>
    /// <returns>Kategoria oraz prawdopodobieństwa dla każdej klasy.</returns>
    /// <response code="200">Zwraca wynik klasyfikacji.</response>
    /// <response code="400">Zły format danych wejściowych.</response>
    /// <response code="500">Błąd przetwarzania na serwerze.</response>
    [HttpPost("analyze")]
    [ProducesResponseType(typeof(ModerationPredictionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AnalyzeText([FromBody] ModerationInput request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Text input is required.");
        }

        try
        {
            var result = _moderationService.Analyze(request.Text);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    }
}

using FinVoice.Contracts.ManualExpense;
using FinVoice.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinVoice.Extensions;

namespace FinVoice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AudioExpensesController : ControllerBase
{
     private readonly VoiceAnalysisService _voiceService;

    public AudioExpensesController(VoiceAnalysisService voiceService)
    {
        _voiceService = voiceService;
    }

    [HttpPost("analyze")]
    [Consumes("multipart/form-data")] 
    public async Task<IActionResult> AnalyzeAudio([FromForm] AudioUploadRequest request)
    {
        if (request.Audio == null || request.Audio.Length == 0)
            return BadRequest("Audio file is required.");

        var result = await _voiceService.AnalyzeAudioAsync(request.Audio, User.GetUserId()!);
        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetAllVoiceAnalysis()
    {
        var result = await _voiceService.GetAllVoiceAnalysis( User.GetUserId()!);
        return Ok(result);
    }
}

using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.ManualExpense;
using FinVoice.Extensions;
using FinVoice.Services.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinVoice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnalysisController(IAnalysisService analysisService) : ControllerBase
{
    private readonly IAnalysisService _analysisService = analysisService;

    [HttpGet("SpendingInsight")]
    public async Task<IActionResult> SpendingInsight()
    {
        var result = await _analysisService.GetSpendingInsightAsync(User.GetUserId()!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

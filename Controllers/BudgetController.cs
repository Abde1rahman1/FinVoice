using Azure.Core;
using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.Budget;
using FinVoice.Extensions;
using FinVoice.Services.BudgetService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FinVoice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetController(IBudgetService budgetService) : ControllerBase
{
    private readonly IBudgetService _budgetService = budgetService;

    [HttpGet]
    public async Task<IActionResult> GetAll ()
    {
        var result = await _budgetService.GetUserBudgetAsync(User.GetUserId()!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult>AddBudget(AddBudgetRequest request,CancellationToken cancellation)
    {
        var result = await _budgetService.AddBudgetAsync(request, User.GetUserId()!, cancellation);
        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpPut("{budgetId}")]
    public async Task<IActionResult> UpdateBudget(AddBudgetRequest request,int budgetId)
    {
        var result = await _budgetService.UpdateBudgetAsync(request, User.GetUserId()!, budgetId);
        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpDelete("{budgetId}")]
    public async Task<IActionResult> DeleteBudget(int budgetId)
    {
        var result = await _budgetService.DeleteBudgetAsync(User.GetUserId()!, budgetId);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("BudgetComparison")]
    public async Task<IActionResult> BudgetComparison()
    {
        var result = await _budgetService.CompareTotalSpendingToBudgetAsync(User.GetUserId()!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}

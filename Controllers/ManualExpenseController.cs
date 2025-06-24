using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.ManualExpense;
using FinVoice.Extensions;
using FinVoice.Services.ExpenseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinVoice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ManualExpenseController(IManualExpenseService expenseService) : ControllerBase
{
    private readonly IManualExpenseService _expenseService = expenseService;

    [HttpPost("")]
    public async Task<IActionResult> Add(ManualExpenseRequest request, CancellationToken cancellationToken)
    {
        var result = await _expenseService.AddExpenseManualAsync(request, User.GetUserId()!, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpPut("{expenseId}")]
    public async Task<IActionResult> Update(ManualExpenseRequest request,int expenseId)
    {
        var result = await _expenseService.UpdateExpenseManualAsync(request, User.GetUserId()!, expenseId);
        return result.IsSuccess ? Created() : result.ToProblem();
    }

    [HttpDelete("{expenseId}")]
    public async Task<IActionResult> Delete(int expenseId)
    {
        var result = await _expenseService.DeleteExpenseAsync( User.GetUserId()!, expenseId);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
}

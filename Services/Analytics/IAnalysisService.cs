using FinVoice.Abstractions.ResultPattern;
using FinVoice.Contracts.Analysis;

namespace FinVoice.Services.Analytics;

public interface IAnalysisService
{
    Task<Result<SpendingInsightResponse>> GetSpendingInsightAsync(string userId);
}

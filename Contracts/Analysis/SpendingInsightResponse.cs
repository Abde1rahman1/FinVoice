namespace FinVoice.Contracts.Analysis;

public record SpendingInsightResponse
(
    decimal CurrentMonthSpent,
    decimal LastMonthSpent,
    string Insight
);

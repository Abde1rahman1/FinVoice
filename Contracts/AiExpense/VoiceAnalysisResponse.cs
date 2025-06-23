namespace FinVoice.Contracts.AiExpense;

public record VoiceAnalysisResponse
(
    int Id,
    string UserId ,
    decimal Amount ,
    string Category ,
    DateTime Date ,
    string? Note 
);

using FinVoice.Contracts.AiExpense;
using FinVoice.Entities;

namespace FinVoice.Services.ExpenseService;

public interface IVoiceAnalysisService
{
    Task<List<Expense>> AnalyzeAudioAsync(IFormFile audioFile, string userId);

    Task<List<VoiceAnalysisResponse>> GetAllVoiceAnalysis(string userId);
}

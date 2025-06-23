namespace FinVoice.Contracts.AiExpense;

public class ExpenseAIResult
{
    public string TranscribedText { get; set; } = "";
    public List<ExpenseItem> Items { get; set; } = new();
}

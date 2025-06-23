namespace FinVoice.Entities;

public class Expense
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } 

    public DateTime Date { get; set; }

    public string? Note { get; set; }

    public string SourceText { get; set; } 

    public User User { get; set; }
}

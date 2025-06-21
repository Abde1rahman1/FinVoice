namespace FinVoice.Entities;

public class Budget
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string Category { get; set; }

    public decimal MonthlyLimit { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; }
}

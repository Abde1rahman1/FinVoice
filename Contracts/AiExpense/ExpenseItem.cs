namespace FinVoice.Contracts.AiExpense;

public class ExpenseItem
{
    public string Product { get; set; } = "";
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => UnitPrice * Quantity;
}

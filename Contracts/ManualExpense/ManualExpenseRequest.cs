using FinVoice.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinVoice.Contracts.ManualExpense;

public record ManualExpenseRequest
(
    decimal Amount,
    string Category,
    DateTime Date,
    string? Note
);

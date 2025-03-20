namespace Backend.Shared.Models.Balance;

public class BalanceDto
{
    public DateTime Date { get; set; }
    public decimal DailyBalance { get; set; }
    public decimal AccumulatedBalance { get; set; }
} 
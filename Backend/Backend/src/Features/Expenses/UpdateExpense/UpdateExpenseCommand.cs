using Backend.Shared.Converters;
using Backend.Shared.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Expenses.UpdateExpense;

public class UpdateExpenseCommand : IRequest<Expense>
{
    public Guid Id { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
} 
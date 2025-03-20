using Backend.Shared.Converters;
using Backend.Shared.Entities;
using Backend.Shared.Models;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Expenses.CreateExpense;

public class CreateExpenseCommand : IRequest<Expense>
{
    public string Type { get; set; } = string.Empty;
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
using Backend.Shared.Enums;
using Backend.Shared.Models;
using Backend.Shared.Extensions;
using Backend.Shared.Converters;
using System.Text.Json.Serialization;

namespace Backend.Shared.Entities;

public class Expense : BaseEntity
{
    private ExpenseType _type = ExpenseType.Despesa;
    
    [JsonIgnore]
    public ExpenseType Type
    {
        get => _type;
        set => _type = value;
    }

    [JsonPropertyName("type")]
    public string TypeString
    {
        get => _type.ToString();
        set => _type = value.ToExpenseType();
    }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
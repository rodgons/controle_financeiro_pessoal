using Backend.Shared.Enums;
using Backend.Shared.Extensions;

namespace Backend.Shared.Models;

public class ExpenseTypeDto
{
    private string _value = "Despesa";
    public string Value
    {
        get => _value;
        set => _value = value;
    }

    public static implicit operator ExpenseType(ExpenseTypeDto dto) => dto.Value.ToExpenseType();
    public static implicit operator ExpenseTypeDto(ExpenseType type) => new() { Value = type.ToString() };
} 
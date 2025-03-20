using Backend.Shared.Enums;

namespace Backend.Shared.Extensions;

public static class ExpenseTypeExtensions
{
    public static string ToString(this ExpenseType type) => type switch
    {
        ExpenseType.Despesa => "Despesa",
        ExpenseType.Receita => "Receita",
        _ => throw new ArgumentException($"Invalid expense type: {type}")
    };

    public static ExpenseType ToExpenseType(this string type) => type.ToLower() switch
    {
        "despesa" => ExpenseType.Despesa,
        "receita" => ExpenseType.Receita,
        _ => throw new ArgumentException($"Invalid expense type string: {type}")
    };
} 
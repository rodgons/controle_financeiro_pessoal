using MediatR;

namespace Backend.Features.Expenses.DeleteExpense;

public record DeleteExpenseCommand(Guid Id) : IRequest<Unit>; 
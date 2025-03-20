using Backend.Shared.Entities;
using MediatR;

namespace Backend.Features.Expenses.GetExpense;

public record GetExpenseQuery(Guid Id) : IRequest<Expense>; 
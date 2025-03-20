using Backend.Infrastructure;
using Backend.Infrastructure.Data;
using Backend.Shared.Exceptions;
using MediatR;

namespace Backend.Features.Expenses.DeleteExpense;

public class DeleteExpenseHandler(AppDbContext context) : IRequestHandler<DeleteExpenseCommand, Unit>
{
    public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await context.Expenses.FindAsync([request.Id], cancellationToken: cancellationToken);
        if (expense == null)
            throw new NotFoundError($"Expense with ID {request.Id} not found");

        context.Expenses.Remove(expense);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
} 
using Backend.Infrastructure;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Exceptions;
using MediatR;

namespace Backend.Features.Expenses.UpdateExpense;

public class UpdateExpenseHandler(AppDbContext context) : IRequestHandler<UpdateExpenseCommand, Expense>
{
    public async Task<Expense> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await context.Expenses.FindAsync(request.Id, cancellationToken);
        if (expense == null)
            throw new NotFoundError($"Expense with ID {request.Id} not found");

        expense.Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);
        expense.Description = request.Description;
        expense.Value = request.Value;

        await context.SaveChangesAsync(cancellationToken);
        return expense;
    }
} 
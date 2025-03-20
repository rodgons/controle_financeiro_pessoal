using Backend.Infrastructure;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Extensions;
using MediatR;

namespace Backend.Features.Expenses.CreateExpense;

public class CreateExpenseHandler(AppDbContext context) : IRequestHandler<CreateExpenseCommand, Expense>
{
    public async Task<Expense> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            TypeString = request.Type,
            Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
            Description = request.Description,
            Value = request.Value
        };

        await context.Expenses.AddAsync(expense, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return expense;
    }
} 
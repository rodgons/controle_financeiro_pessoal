using Backend.Infrastructure;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using MediatR;

namespace Backend.Features.Expenses.GetExpense;

public class GetExpenseHandler(AppDbContext context) : IRequestHandler<GetExpenseQuery, Expense?>
{
    public async Task<Expense?> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        return await context.Expenses.FindAsync(request.Id, cancellationToken);
    }
} 
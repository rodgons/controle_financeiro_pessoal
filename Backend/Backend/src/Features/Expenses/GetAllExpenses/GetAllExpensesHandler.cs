using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Extensions;
using Backend.Shared.Models.Pagination;
using MediatR;

namespace Backend.Features.Expenses.GetAllExpenses;

public class GetAllExpensesHandler(AppDbContext context) : IRequestHandler<GetAllExpensesQuery, PaginationResponse<Expense>>
{
    public async Task<PaginationResponse<Expense>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        return await context.Expenses
            .OrderBy(e => e.Id)
            .ToPaginationResponseAsync(request, cancellationToken);
    }
} 
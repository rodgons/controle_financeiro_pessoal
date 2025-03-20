using Backend.Infrastructure.Data;
using Backend.Shared.Models.Balance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Expenses.GetBalance;

public class GetBalanceHandler(AppDbContext context) : IRequestHandler<GetBalanceQuery, IEnumerable<BalanceDto>>
{
    public async Task<IEnumerable<BalanceDto>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        var dailyBalances = await context.Expenses
            .Where(e => e.Date.Date >= request.StartDate.Date && e.Date.Date <= request.EndDate.Date)
            .GroupBy(e => e.Date.Date)
            .Select(g => new
            {
                Date = g.Key,
                DailyBalance = g.Sum(e => e.Type == Backend.Shared.Enums.ExpenseType.Receita ? e.Value : -e.Value)
            })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        var accumulatedBalance = 0m;
        return dailyBalances.Select(x =>
        {
            accumulatedBalance += x.DailyBalance;
            return new BalanceDto
            {
                Date = x.Date,
                DailyBalance = x.DailyBalance,
                AccumulatedBalance = accumulatedBalance
            };
        });
    }
} 
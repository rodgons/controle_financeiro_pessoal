using Backend.Shared.Models.Balance;
using MediatR;

namespace Backend.Features.Expenses.GetBalance;

public record GetBalanceQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<BalanceDto>>; 
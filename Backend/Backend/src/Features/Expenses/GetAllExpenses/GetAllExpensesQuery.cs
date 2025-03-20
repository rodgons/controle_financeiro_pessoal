using Backend.Shared.Entities;
using Backend.Shared.Models.Pagination;
using MediatR;

namespace Backend.Features.Expenses.GetAllExpenses;

public record GetAllExpensesQuery : PaginationRequest, IRequest<PaginationResponse<Expense>>; 
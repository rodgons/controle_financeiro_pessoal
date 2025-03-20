using Backend.Features.Expenses.CreateExpense;
using Backend.Features.Expenses.DeleteExpense;
using Backend.Features.Expenses.GetAllExpenses;
using Backend.Features.Expenses.GetBalance;
using Backend.Features.Expenses.GetExpense;
using Backend.Features.Expenses.UpdateExpense;
using Backend.Shared.Entities;
using Backend.Shared.Interfaces;
using Backend.Shared.Models;
using Backend.Shared.Models.Balance;
using MediatR;
using Microsoft.OpenApi.Models;

namespace Backend.Features.Expenses;

public class ExpensesEndpoints : IEndpoint
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/expenses", async (CreateExpenseCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command, cancellationToken);
            return Results.Created($"/expenses/{result.Id}", result);
        })
        .WithName("CreateExpense")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Creates a new expense",
            Description = "Creates a new expense with the provided details"
        });

        app.MapGet("/expenses/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var query = new GetExpenseQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetExpenseById")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Gets an expense by ID",
            Description = "Retrieves a specific expense by its unique identifier"
        })
        .Produces<Expense>()
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/expenses", async (int? pageSize, int? pageNumber, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var query = new GetAllExpensesQuery
            {
                PageSize = pageSize ?? 10,
                PageNumber = pageNumber ?? 1
            };
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetAllExpenses")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Gets all expenses",
            Description = "Retrieves a paginated list of all expenses"
        })
        .Produces<PagedResult<Expense>>();

        app.MapPut("/expenses/{id:guid}", async (Guid id, UpdateExpenseCommand command, IMediator mediator, CancellationToken cancellationToken) =>
        {
            command.Id = id;
            var result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("UpdateExpense")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Updates an expense",
            Description = "Updates an existing expense with the provided details"
        })
        .Produces<Expense>()
        .Produces(StatusCodes.Status404NotFound);

        app.MapDelete("/expenses/{id:guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new DeleteExpenseCommand(id);
            await mediator.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .WithName("DeleteExpense")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Deletes an expense",
            Description = "Deletes an expense by its unique identifier"
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/expenses/balance", async (DateTime startDate, DateTime endDate, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var query = new GetBalanceQuery(startDate, endDate);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetExpenseBalance")
        .WithOpenApi(operation => new OpenApiOperation(operation)
        {
            Summary = "Gets the balance for a date range",
            Description = "Calculates and returns the balance for expenses within the specified date range"
        })
        .Produces<IEnumerable<BalanceDto>>();
    }
}
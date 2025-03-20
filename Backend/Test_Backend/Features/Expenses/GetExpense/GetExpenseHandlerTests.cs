using Backend.Features.Expenses.GetExpense;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Test_Backend.Infrastructure.Data;

namespace Test_Backend.Features.Expenses.GetExpense;

public class GetExpenseHandlerTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly GetExpenseHandler _handler;

    public GetExpenseHandlerTests()
    {
        _context = new TestAppDbContext();
        _handler = new GetExpenseHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_WithExistingId_ReturnsExpense()
    {
        // Arrange
        var expenseId = Guid.CreateVersion7();
        var expense = new Expense 
        { 
            Id = expenseId,
            Description = "Test Expense", 
            Value = 100, 
            Date = DateTime.UtcNow 
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new GetExpenseQuery(expenseId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expenseId, result.Id);
        Assert.Equal(expense.Description, result.Description);
        Assert.Equal(expense.Value, result.Value);
        Assert.Equal(expense.Date, result.Date);
    }

    [Fact]
    public async Task Handle_WithNonExistingId_ReturnsNull()
    {
        // Arrange
        var expenseId = Guid.CreateVersion7();

        // Act
        var result = await _handler.Handle(new GetExpenseQuery(expenseId), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
} 
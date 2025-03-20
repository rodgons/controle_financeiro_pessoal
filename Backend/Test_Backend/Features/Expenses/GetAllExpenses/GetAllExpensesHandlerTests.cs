using Backend.Features.Expenses.GetAllExpenses;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Test_Backend.Infrastructure.Data;

namespace Test_Backend.Features.Expenses.GetAllExpenses;

public class GetAllExpensesHandlerTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly GetAllExpensesHandler _handler;

    public GetAllExpensesHandlerTests()
    {
        _context = new TestAppDbContext();
        _handler = new GetAllExpensesHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private async Task CleanupDatabaseAsync()
    {
        _context.Expenses.RemoveRange(_context.Expenses);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_WithDefaultPagination_ReturnsFirstPageWithTenItems()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var expenses = Enumerable.Range(1, 15)
            .Select(i => new Expense 
            { 
                Id = Guid.CreateVersion7(),
                TypeString = i % 2 == 0 ? "Despesa" : "Receita",
                Description = $"Expense {i}", 
                Value = i * 100, 
                Date = DateTime.UtcNow 
            })
            .ToList();

        _context.Expenses.AddRange(expenses);
        await _context.SaveChangesAsync();

        var query = new GetAllExpensesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(15, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
        Assert.Equal(10, result.Items.Count());
    }

    [Fact]
    public async Task Handle_WithCustomPagination_ReturnsCorrectPage()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var expenses = Enumerable.Range(1, 25)
            .Select(i => new Expense 
            { 
                Id = Guid.CreateVersion7(),
                TypeString = i % 2 == 0 ? "Despesa" : "Receita",
                Description = $"Expense {i}", 
                Value = i * 100, 
                Date = DateTime.UtcNow 
            })
            .ToList();

        _context.Expenses.AddRange(expenses);
        await _context.SaveChangesAsync();

        var query = new GetAllExpensesQuery { PageSize = 5, PageNumber = 3 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.PageNumber);
        Assert.Equal(5, result.PageSize);
        Assert.Equal(25, result.TotalCount);
        Assert.Equal(5, result.TotalPages);
        Assert.True(result.HasNextPage);
        Assert.True(result.HasPreviousPage);
        Assert.Equal(5, result.Items.Count());
    }

    [Fact]
    public async Task Handle_WithEmptyDatabase_ReturnsEmptyPage()
    {
        // Arrange
        await CleanupDatabaseAsync();
        var query = new GetAllExpensesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasNextPage);
        Assert.False(result.HasPreviousPage);
    }
} 
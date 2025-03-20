using Backend.Features.Expenses.GetBalance;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Test_Backend.Infrastructure.Data;

namespace Test_Backend.Features.Expenses.GetBalance;

public class GetBalanceHandlerTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly GetBalanceHandler _handler;

    public GetBalanceHandlerTests()
    {
        _context = new TestAppDbContext();
        _handler = new GetBalanceHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_WithExpensesInPeriod_ReturnsCorrectBalance()
    {
        // Arrange
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 3, 3);

        var expenses = new List<Expense>
        {
            new()
            {
                TypeString = "Receita",
                Description = "Salary",
                Value = 5000,
                Date = new DateTime(2024, 3, 1)
            },
            new()
            {
                TypeString = "Despesa",
                Description = "Rent",
                Value = 1000,
                Date = new DateTime(2024, 3, 1)
            },
            new()
            {
                TypeString = "Despesa",
                Description = "Groceries",
                Value = 500,
                Date = new DateTime(2024, 3, 2)
            },
            new()
            {
                TypeString = "Receita",
                Description = "Freelance",
                Value = 1000,
                Date = new DateTime(2024, 3, 3)
            }
        };

        await _context.Expenses.AddRangeAsync(expenses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new GetBalanceQuery(startDate, endDate), CancellationToken.None);
        var balances = result.ToList();

        // Assert
        Assert.Equal(3, balances.Count);

        // Day 1: +5000 (Salary) - 1000 (Rent) = 4000
        Assert.Equal(new DateTime(2024, 3, 1), balances[0].Date);
        Assert.Equal(4000, balances[0].DailyBalance);
        Assert.Equal(4000, balances[0].AccumulatedBalance);

        // Day 2: -500 (Groceries)
        Assert.Equal(new DateTime(2024, 3, 2), balances[1].Date);
        Assert.Equal(-500, balances[1].DailyBalance);
        Assert.Equal(3500, balances[1].AccumulatedBalance);

        // Day 3: +1000 (Freelance)
        Assert.Equal(new DateTime(2024, 3, 3), balances[2].Date);
        Assert.Equal(1000, balances[2].DailyBalance);
        Assert.Equal(4500, balances[2].AccumulatedBalance);
    }

    [Fact]
    public async Task Handle_WithNoExpensesInPeriod_ReturnsEmptyList()
    {
        // Arrange
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 3, 3);

        // Act
        var result = await _handler.Handle(new GetBalanceQuery(startDate, endDate), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithExpensesOutsidePeriod_ReturnsEmptyList()
    {
        // Arrange
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 3, 3);

        var expenses = new List<Expense>
        {
            new()
            {
                TypeString = "Receita",
                Description = "Salary",
                Value = 5000,
                Date = new DateTime(2024, 2, 28)
            },
            new()
            {
                TypeString = "Despesa",
                Description = "Rent",
                Value = 1000,
                Date = new DateTime(2024, 3, 4)
            }
        };

        await _context.Expenses.AddRangeAsync(expenses);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new GetBalanceQuery(startDate, endDate), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
} 
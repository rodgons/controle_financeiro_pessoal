using Backend.Features.Expenses.CreateExpense;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Test_Backend.Infrastructure.Data;

namespace Test_Backend.Features.Expenses.CreateExpense;

public class CreateExpenseHandlerTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly CreateExpenseHandler _handler;

    public CreateExpenseHandlerTests()
    {
        _context = new TestAppDbContext();
        _handler = new CreateExpenseHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesExpense()
    {
        // Arrange
        var command = new CreateExpenseCommand
        {
            Type = "Despesa",
            Description = "Test Expense",
            Value = 100,
            Date = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(ExpenseType.Despesa, result.Type);
        Assert.Equal("Despesa", result.TypeString);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Value, result.Value);
        Assert.Equal(command.Date, result.Date);

        var savedExpense = await _context.Expenses.FindAsync(result.Id);
        Assert.NotNull(savedExpense);
        Assert.Equal(ExpenseType.Despesa, savedExpense.Type);
        Assert.Equal("Despesa", savedExpense.TypeString);
        Assert.Equal(result.Description, savedExpense.Description);
        Assert.Equal(result.Value, savedExpense.Value);
        Assert.Equal(result.Date, savedExpense.Date);
    }

    [Fact]
    public async Task Handle_WithReceitaType_CreatesReceita()
    {
        // Arrange
        var command = new CreateExpenseCommand
        {
            Type = "Receita",
            Description = "Test Receita",
            Value = 1000,
            Date = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ExpenseType.Receita, result.Type);
        Assert.Equal("Receita", result.TypeString);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.Value, result.Value);
        Assert.Equal(command.Date, result.Date);

        var savedExpense = await _context.Expenses.FindAsync(result.Id);
        Assert.NotNull(savedExpense);
        Assert.Equal(ExpenseType.Receita, savedExpense.Type);
        Assert.Equal("Receita", savedExpense.TypeString);
    }

    [Fact]
    public async Task Handle_WithInvalidType_ThrowsArgumentException()
    {
        // Arrange
        var command = new CreateExpenseCommand
        {
            Type = "InvalidType",
            Description = "Test Expense",
            Value = 100,
            Date = DateTime.UtcNow
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
} 
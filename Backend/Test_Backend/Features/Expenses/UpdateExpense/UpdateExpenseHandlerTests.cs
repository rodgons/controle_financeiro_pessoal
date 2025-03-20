using Backend.Features.Expenses.UpdateExpense;
using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Backend.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Test_Backend.Infrastructure.Data;

namespace Test_Backend.Features.Expenses.UpdateExpense;

public class UpdateExpenseHandlerTests : IDisposable
{
    private readonly TestAppDbContext _context;
    private readonly UpdateExpenseHandler _handler;

    public UpdateExpenseHandlerTests()
    {
        _context = new TestAppDbContext();
        _handler = new UpdateExpenseHandler(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Handle_WithNonExistingId_ThrowsNotFoundError()
    {
        // Arrange
        var command = new UpdateExpenseCommand
        {
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow,
            Description = "Test",
            Value = 100
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundError>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
} 
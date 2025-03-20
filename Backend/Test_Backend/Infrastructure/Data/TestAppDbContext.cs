using Backend.Infrastructure.Data;
using Backend.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test_Backend.Infrastructure.Data;

public sealed class TestAppDbContext : AppDbContext
{
    public TestAppDbContext() : base(
        new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options)
    {
        Database.EnsureCreated();
    }
} 
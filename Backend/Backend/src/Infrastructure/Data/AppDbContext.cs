using Backend.Shared.Entities;
using Backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Value).HasPrecision(18, 2);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);
        });

        modelBuilder.Entity<Expense>().HasData(
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 8, 29, 0, 0, 0, DateTimeKind.Utc), Description = "Cartão de Crédito", Value = 825.82m, CreatedAt = new DateTime(2022, 8, 29, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 8, 29, 0, 0, 0, DateTimeKind.Utc), Description = "Curso C#", Value = 200.00m, CreatedAt = new DateTime(2022, 8, 29, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Receita, Date = new DateTime(2022, 8, 31, 0, 0, 0, DateTimeKind.Utc), Description = "Salário", Value = 7000.00m, CreatedAt = new DateTime(2022, 8, 31, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), Description = "Mercado", Value = 3000.00m, CreatedAt = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), Description = "Farmácia", Value = 300.00m, CreatedAt = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc), Description = "Combustível", Value = 800.25m, CreatedAt = new DateTime(2022, 9, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc), Description = "Financiamento Carro", Value = 900.00m, CreatedAt = new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Despesa, Date = new DateTime(2022, 9, 22, 0, 0, 0, DateTimeKind.Utc), Description = "Financiamento Casa", Value = 1200.00m, CreatedAt = new DateTime(2022, 9, 22, 0, 0, 0, DateTimeKind.Utc) },
            new Expense { Type = ExpenseType.Receita, Date = new DateTime(2022, 9, 25, 0, 0, 0, DateTimeKind.Utc), Description = "Freelance Projeto XPTO", Value = 2500.00m, CreatedAt = new DateTime(2022, 9, 25, 0, 0, 0, DateTimeKind.Utc) }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Modified });

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
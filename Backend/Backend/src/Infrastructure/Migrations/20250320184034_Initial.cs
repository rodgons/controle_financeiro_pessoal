using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.src.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TypeString = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "CreatedAt", "Date", "Description", "Type", "TypeString", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { new Guid("0195b4db-6f40-703f-b92d-0df9acba6e59"), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Combustível", 0, "Despesa", null, 800.25m },
                    { new Guid("0195b4db-6f40-72b8-8d89-4a6099919a3e"), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), "Salário", 1, "Receita", null, 7000.00m },
                    { new Guid("0195b4db-6f40-748c-8c5e-d3343d030da8"), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Farmácia", 0, "Despesa", null, 300.00m },
                    { new Guid("0195b4db-6f40-774b-b696-9cc94077f8d8"), new DateTime(2022, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Freelance Projeto XPTO", 1, "Receita", null, 2500.00m },
                    { new Guid("0195b4db-6f40-7857-a036-80a6a7fbb5dd"), new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Financiamento Carro", 0, "Despesa", null, 900.00m },
                    { new Guid("0195b4db-6f40-7a11-8bee-e9780f7d7a81"), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mercado", 0, "Despesa", null, 3000.00m },
                    { new Guid("0195b4db-6f40-7aaa-b868-f269fed2336a"), new DateTime(2022, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Cartão de Crédito", 0, "Despesa", null, 825.82m },
                    { new Guid("0195b4db-6f40-7d93-9b66-da43af0df875"), new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Financiamento Casa", 0, "Despesa", null, 1200.00m },
                    { new Guid("0195b4db-6f40-7e49-a80c-36e06b0615c9"), new DateTime(2022, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Curso C#", 0, "Despesa", null, 200.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");
        }
    }
}

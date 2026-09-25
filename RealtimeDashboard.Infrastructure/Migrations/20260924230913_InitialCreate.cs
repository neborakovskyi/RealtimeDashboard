using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealtimeDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Metrics",
                columns: new[] { "Id", "Category", "Key", "Label", "Unit", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { new Guid("1ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 2, "orders_today", "Orders today", "#", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 },
                    { new Guid("2ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 2, "revenue_today", "Income today", "$", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 },
                    { new Guid("3ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 3, "ai_tasks_queue", "AI task in queu", "#", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 },
                    { new Guid("4ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 3, "ai_tasks_done", "AI task done", "#", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 },
                    { new Guid("5ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 4, "cpu_usage", "CPU loading", "%", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 },
                    { new Guid("7ccd0dc7-d73c-41b0-a734-222d00e25bdc"), 1, "active_users", "Active users", "#", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_Key",
                table: "Metrics",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}

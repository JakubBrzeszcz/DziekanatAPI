using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "828a0994-4119-4b5f-91df-68ef1f4c20c1", "AQAAAAIAAYagAAAAEP9hZtIDj9NvMQ9gD40Tu5WZOX6bs4RsdvRDO3vSya4hHhmZcggLN2QI4+6Jyx1fYw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "E244194A-4254-434A-8E9A-912784462A05",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "92b0484f-59a1-48e8-8e1a-4e66f1f3f727", "AQAAAAIAAYagAAAAEOb4kK9GkcD9TRUFeB86rqM5C76dIkzUunO+2ukeuxN7Z1aM3vYnaQ2BbJEtkTQtlg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a596707a-e710-41ce-90a5-3a8a25a835f1", "AQAAAAIAAYagAAAAEE+xC98O3NWJHBCbhYq+0IS5ihK2MmOPAHL0eM3ivgrSZqzgkbyXkCGvms+kEBV5+Q==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "E244194A-4254-434A-8E9A-912784462A05",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5fb05a8f-2873-45d3-a6d9-d07413316e25", "AQAAAAIAAYagAAAAEAjM5sfS83ROpTUAZXEF0e7gglYry7dkP/zoGrpnPJX6SOvxODYzNEkkabTk3IGalQ==" });
        }
    }
}

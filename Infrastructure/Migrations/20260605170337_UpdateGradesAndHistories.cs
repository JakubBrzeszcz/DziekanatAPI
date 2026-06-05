using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGradesAndHistories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GradeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OldValue = table.Column<double>(type: "REAL", nullable: false),
                    NewValue = table.Column<double>(type: "REAL", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeHistories_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cbdfb8e9-3866-415f-ae65-4024d02223bd", "AQAAAAIAAYagAAAAEAsEU3fPKyHpxMEHy1tdxmuWsoLABDlqW/xae0XtlFQZIMLQcsAG4Ij+ZHdDvK/uWw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "E244194A-4254-434A-8E9A-912784462A05",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e199dc92-e508-4759-bcc0-98ae0d065ee8", "AQAAAAIAAYagAAAAEBH9mP06bTYDRDM15wP5bScb2eckdDj4pHO7vAfe893MsVm/uI60Q0cGpdaF1WPCTA==" });

            migrationBuilder.CreateIndex(
                name: "IX_GradeHistories_GradeId",
                table: "GradeHistories",
                column: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeHistories");

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
    }
}

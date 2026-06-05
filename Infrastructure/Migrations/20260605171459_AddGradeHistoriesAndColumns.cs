using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeHistoriesAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c7776b5-07bb-484d-bcaa-91cb7b7b437e", "AQAAAAIAAYagAAAAEGwJ14yiYcWL3il6XcjskuHAXFo94QVQte55oDKbbprsCMwOyRaDZCKNgdI4HILVNw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "E244194A-4254-434A-8E9A-912784462A05",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f4dfcb68-0337-4949-b2ba-030c67416131", "AQAAAAIAAYagAAAAEMGw/7Q3VI6ZmUDPMUqnAcuteamGBldzlq1fhEcYer4rzsFmKlzHPZAMCdRzhcn/qA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

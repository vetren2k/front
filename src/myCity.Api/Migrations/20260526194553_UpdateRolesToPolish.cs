using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myCity.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRolesToPolish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE users SET role = 'Mieszkaniec' WHERE role = 'Resident';");
            migrationBuilder.Sql("UPDATE users SET role = 'Urzędnik' WHERE role = 'Official';");
            migrationBuilder.Sql("UPDATE users SET role = 'Wykonawca' WHERE role = 'Contractor';");
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8222), "Mieszkaniec" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8228), "Mieszkaniec" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8230), "Mieszkaniec" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8232), "Mieszkaniec" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8234), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8236), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8305), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8307), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8309), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8310), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8312), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8314), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8316), "Urzędnik" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8317), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8320), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8322), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8324), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8326), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8327), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8329), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8331), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8333), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8335), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8337), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8339), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8341), "Wykonawca" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 26, 19, 45, 52, 632, DateTimeKind.Utc).AddTicks(8343), "Wykonawca" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE users SET role = 'Resident' WHERE role = 'Mieszkaniec';");
            migrationBuilder.Sql("UPDATE users SET role = 'Official' WHERE role = 'Urzędnik';");
            migrationBuilder.Sql("UPDATE users SET role = 'Contractor' WHERE role = 'Wykonawca';");
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3668), "Resident" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3675), "Resident" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3677), "Resident" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3678), "Resident" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3680), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3682), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3684), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3686), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3687), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3689), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3691), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3692), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3694), "Official" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3696), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3698), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3700), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3702), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 18,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3704), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 19,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3706), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 20,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3707), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 21,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3709), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 22,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3711), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 23,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3713), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 24,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3743), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 25,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3745), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 26,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3747), "Contractor" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 27,
                columns: new[] { "creation_timestamp", "role" },
                values: new object[] { new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3749), "Contractor" });
        }
    }
}

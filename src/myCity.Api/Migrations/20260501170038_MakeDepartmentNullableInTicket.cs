using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myCity.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeDepartmentNullableInTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_public_body_departments_public_body_department_id",
                table: "tickets");

            migrationBuilder.AlterColumn<int>(
                name: "public_body_department_id",
                table: "tickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3668));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3675));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3677));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3678));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3682));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3684));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 8,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3686));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 9,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 10,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3689));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 11,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3691));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 12,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 13,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3694));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 14,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3696));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 15,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 16,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 17,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 18,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 19,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3706));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 20,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 21,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 22,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3711));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 23,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 24,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3743));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 25,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3745));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 26,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 27,
                column: "creation_timestamp",
                value: new DateTime(2026, 5, 1, 17, 0, 37, 793, DateTimeKind.Utc).AddTicks(3749));

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_public_body_departments_public_body_department_id",
                table: "tickets",
                column: "public_body_department_id",
                principalTable: "public_body_departments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tickets_public_body_departments_public_body_department_id",
                table: "tickets");

            migrationBuilder.AlterColumn<int>(
                name: "public_body_department_id",
                table: "tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 4,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2464));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 5,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2466));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 6,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2468));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 7,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2470));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 8,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2471));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 9,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 10,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2475));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 11,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2477));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 12,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2478));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 13,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2480));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 14,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2482));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 15,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2484));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 16,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2486));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 17,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2488));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 18,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2490));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 19,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2492));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 20,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2494));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 21,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2496));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 22,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2549));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 23,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2551));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 24,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2553));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 25,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2555));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 26,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2557));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 27,
                column: "creation_timestamp",
                value: new DateTime(2026, 4, 17, 19, 57, 32, 341, DateTimeKind.Utc).AddTicks(2559));

            migrationBuilder.AddForeignKey(
                name: "fk_tickets_public_body_departments_public_body_department_id",
                table: "tickets",
                column: "public_body_department_id",
                principalTable: "public_body_departments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

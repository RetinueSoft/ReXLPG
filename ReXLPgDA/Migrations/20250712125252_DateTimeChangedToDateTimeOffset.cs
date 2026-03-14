using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReXLPgDA.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeChangedToDateTimeOffset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RegisterDate",
                table: "User",
                type: "datetimeoffset(3)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfLeft",
                table: "User",
                type: "datetimeoffset(3)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfJoining",
                table: "User",
                type: "datetimeoffset(3)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateOfBirth",
                table: "User",
                type: "datetimeoffset(3)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PostedOn",
                table: "Comments",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "GUID",
                keyValue: new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"),
                columns: new[] { "DateOfBirth", "DateOfJoining", "DateOfLeft", "RegisterDate" },
                values: new object[] { null, new DateTimeOffset(new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(3)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfLeft",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfJoining",
                table: "User",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(3)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostedOn",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "GUID",
                keyValue: new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"),
                columns: new[] { "DateOfBirth", "DateOfJoining", "DateOfLeft", "RegisterDate" },
                values: new object[] { null, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}

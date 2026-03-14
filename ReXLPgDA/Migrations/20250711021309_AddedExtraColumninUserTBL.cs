using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReXLPgDA.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraColumninUserTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserImages",
                table: "User",
                newName: "UserImage");

            migrationBuilder.AddColumn<string>(
                name: "AadharImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact1Mobile",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact1Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact2Mobile",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact2Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurposeOfStay",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkIdImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPlaceName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingPlaceId",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "GUID",
                keyValue: new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"),
                columns: new[] { "AadharImage", "Address", "EmergencyContact1Mobile", "EmergencyContact1Name", "EmergencyContact2Mobile", "EmergencyContact2Name", "PurposeOfStay", "UserImage", "WorkIdImage", "WorkPlaceName", "WorkingPlaceId" },
                values: new object[] { null, null, null, null, null, null, null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharImage",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmergencyContact1Mobile",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmergencyContact1Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmergencyContact2Mobile",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmergencyContact2Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PurposeOfStay",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkIdImage",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkPlaceName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkingPlaceId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserImage",
                table: "User",
                newName: "UserImages");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "GUID",
                keyValue: new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"),
                column: "UserImages",
                value: "");
        }
    }
}

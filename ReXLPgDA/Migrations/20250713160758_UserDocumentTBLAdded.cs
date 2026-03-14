using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReXLPgDA.Migrations
{
    /// <inheritdoc />
    public partial class UserDocumentTBLAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharImage",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WorkIdImage",
                table: "User");

            migrationBuilder.CreateTable(
                name: "UserDocument",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AadharFrontImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AadharBackImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    WorkIdFrontImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    WorkIdBackImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocument", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserDocument_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDocument");

            migrationBuilder.AddColumn<string>(
                name: "AadharImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "User",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkIdImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "GUID",
                keyValue: new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"),
                columns: new[] { "AadharImage", "UserImage", "WorkIdImage" },
                values: new object[] { null, null, null });
        }
    }
}

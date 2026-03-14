using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReXLPgDA.Migrations
{
    /// <inheritdoc />
    public partial class UserTBLWithDefaults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginAllowed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<int>(type: "int", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfLeft = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserImages = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.GUID);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "GUID", "About", "Active", "DateOfBirth", "DateOfJoining", "DateOfLeft", "Designation", "DisplayName", "Gender", "LoginAllowed", "Mobile", "Name", "Password", "RegisterDate", "UserImages" },
                values: new object[] { new Guid("24090df0-2507-44e1-8c5b-c1087a7c64a4"), "", true, null, new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "Admin", 0, true, "9943135008", "Redmin", "Red123!@#", new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

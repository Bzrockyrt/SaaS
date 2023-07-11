using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Update_DB_ENTREPRISETEST : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DevNote",
                table: "Log",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExceptionName",
                table: "Log",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LogType",
                table: "Log",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Log",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Log",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDemo",
                table: "CompanySetting",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevNote",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "ExceptionName",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "LogType",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "IsDemo",
                table: "CompanySetting");
        }
    }
}

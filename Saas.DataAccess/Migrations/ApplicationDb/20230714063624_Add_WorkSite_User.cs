using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Add_WorkSite_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkHour",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHour_UserId",
                table: "WorkHour",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHour_AspNetUsers_UserId",
                table: "WorkHour",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkHour_AspNetUsers_UserId",
                table: "WorkHour");

            migrationBuilder.DropIndex(
                name: "IX_WorkHour_UserId",
                table: "WorkHour");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkHour");
        }
    }
}

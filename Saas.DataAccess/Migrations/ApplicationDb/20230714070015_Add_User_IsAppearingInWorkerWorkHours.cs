using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Add_User_IsAppearingInWorkerWorkHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAppearingInWorkerWorkHours",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAppearingInWorkerWorkHours",
                table: "AspNetUsers");
        }
    }
}

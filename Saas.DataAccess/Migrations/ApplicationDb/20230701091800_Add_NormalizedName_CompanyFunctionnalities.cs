using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Add_NormalizedName_CompanyFunctionnalities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "CompanyFunctionnalities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "CompanyFunctionnalities");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Add_Job_CompanyFunctionnalities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SocialSecurityNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Job_CompanyFunctionnalities",
                columns: table => new
                {
                    JobId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyFunctionnalitiesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job_CompanyFunctionnalities", x => new { x.JobId, x.CompanyFunctionnalitiesId });
                    table.ForeignKey(
                        name: "FK_Job_CompanyFunctionnalities_CompanyFunctionnalities_CompanyFunctionnalitiesId",
                        column: x => x.CompanyFunctionnalitiesId,
                        principalTable: "CompanyFunctionnalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Job_CompanyFunctionnalities_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_CompanyFunctionnalities_CompanyFunctionnalitiesId",
                table: "Job_CompanyFunctionnalities",
                column: "CompanyFunctionnalitiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job_CompanyFunctionnalities");

            migrationBuilder.AlterColumn<string>(
                name: "SocialSecurityNumber",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

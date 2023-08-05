using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class Change_Creator_AllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkSiteType",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkSite",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkHourImage",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkHour_WorkSite",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkHour",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "UserStatus",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Supplier_Article",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Supplier",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Subsidiary",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Log",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Job",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "EmploymentContract",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Department",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CompanySetting",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CompanyPicture",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "CompanyFunctionnalities",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ArticleImage",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Article",
                newName: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "WorkSiteType",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "WorkSite",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "WorkHourImage",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "WorkHour_WorkSite",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "WorkHour",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "UserStatus",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Supplier_Article",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Supplier",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Subsidiary",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Log",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Job",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "EmploymentContract",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Department",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "CompanySetting",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "CompanyPicture",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "CompanyFunctionnalities",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "ArticleImage",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Article",
                newName: "CreatedBy");
        }
    }
}

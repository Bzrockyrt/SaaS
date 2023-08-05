using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Change_Creator_AllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0198a5cc-9db5-4e58-bd86-72d7f67ff311");

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: "eaf4782d-d9da-48a7-8a96-420fba52a0cc");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Log",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Functionnality",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Company",
                newName: "CreatorId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "Discriminator", "Email", "EmailConfirmed", "Firstname", "IsEnable", "IsSuperUser", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { "ada79e95-ecac-4b80-85dd-34e55b32580a", 0, "3194578b-ea47-4edc-94c4-77da99bbfca3", "Pierre-Louis IPPOLITI", new DateTime(2023, 7, 13, 21, 44, 13, 122, DateTimeKind.Local).AddTicks(5929), "User", "pierrelouisippoliti@pipl-developpement.com", false, "Pierre-Louis", true, true, "IPPOLITI", false, null, null, null, "AQAAAAIAAYagAAAAEERgZUqstg7yx/qOMW2RTAVmF3eZn1LPNRXE9WMk+zpKRJTXaEG4Kd4q0y5yiIE7nQ==", null, false, "24f72a71-fbc6-4ce1-a588-188169cd0267", false, "", null, "PlIppoliti" });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "CompanyCode", "CreatedOn", "CreatorId", "Description", "Email", "IsEnable", "IsSuperCompany", "Name", "PhoneNumber", "PostalCode", "SIRET", "State", "StreetName", "StreetNumber", "TenantCode", "UpdatedBy", "UpdatedOn" },
                values: new object[] { "6f840818-2d82-4919-8b83-f633c424beb0", "PIPL0001", new DateTime(2023, 7, 13, 21, 44, 13, 122, DateTimeKind.Local).AddTicks(5803), "", "Entreprise PIPL Développement", "pierrelouisippoliti@pipl-developpement.com", true, true, "PIPL Développement", 633333799.0, 1380L, 91911872900012.0, "Saint André-de-Bâgé", "rue du Villard", 181L, "pipl-developpement", "", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ada79e95-ecac-4b80-85dd-34e55b32580a");

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: "6f840818-2d82-4919-8b83-f633c424beb0");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Log",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Functionnality",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Company",
                newName: "CreatedBy");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "Discriminator", "Email", "EmailConfirmed", "Firstname", "IsEnable", "IsSuperUser", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { "0198a5cc-9db5-4e58-bd86-72d7f67ff311", 0, "62f04791-b1bd-46a7-a3ee-707c05753d9b", "Pierre-Louis IPPOLITI", new DateTime(2023, 7, 3, 17, 39, 43, 771, DateTimeKind.Local).AddTicks(5153), "User", "pierrelouisippoliti@pipl-developpement.com", false, "Pierre-Louis", true, true, "IPPOLITI", false, null, null, null, "AQAAAAIAAYagAAAAEGxD/ZpqYMnXS3M3n2FBh5W2FF6ynreJ5fqrReDtPVdAKyfqIKhqiDoQvzOnfudoQw==", null, false, "1b876546-99f1-47ca-9ff9-5f0786970082", false, "", null, "PlIppoliti" });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "CompanyCode", "CreatedBy", "CreatedOn", "Description", "Email", "IsEnable", "IsSuperCompany", "Name", "PhoneNumber", "PostalCode", "SIRET", "State", "StreetName", "StreetNumber", "TenantCode", "UpdatedBy", "UpdatedOn" },
                values: new object[] { "eaf4782d-d9da-48a7-8a96-420fba52a0cc", "PIPL0001", "Pierre-Louis IPPOLITI", new DateTime(2023, 7, 3, 17, 39, 43, 771, DateTimeKind.Local).AddTicks(5028), "Entreprise PIPL Développement", "pierrelouisippoliti@pipl-developpement.com", true, true, "PIPL Développement", 633333799.0, 1380L, 91911872900012.0, "Saint André-de-Bâgé", "rue du Villard", 181L, "pipl-developpement", "", null });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class shkuh111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanies_Accounts_AccountsId",
                table: "UserCompanies");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanies_AccountsId",
                table: "UserCompanies");

            migrationBuilder.DropColumn(
                name: "AccountsId",
                table: "UserCompanies");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanies_AccountId",
                table: "UserCompanies",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanies_Accounts_AccountId",
                table: "UserCompanies",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanies_Accounts_AccountId",
                table: "UserCompanies");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanies_AccountId",
                table: "UserCompanies");

            migrationBuilder.AddColumn<int>(
                name: "AccountsId",
                table: "UserCompanies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanies_AccountsId",
                table: "UserCompanies",
                column: "AccountsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanies_Accounts_AccountsId",
                table: "UserCompanies",
                column: "AccountsId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

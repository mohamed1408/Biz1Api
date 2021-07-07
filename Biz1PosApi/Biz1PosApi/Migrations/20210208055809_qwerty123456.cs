using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qwerty123456 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorePaymentTypeId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StorePaymentTypeId",
                table: "Transactions",
                column: "StorePaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_StorePaymentTypes_StorePaymentTypeId",
                table: "Transactions",
                column: "StorePaymentTypeId",
                principalTable: "StorePaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_StorePaymentTypes_StorePaymentTypeId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_StorePaymentTypeId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "StorePaymentTypeId",
                table: "Transactions");
        }
    }
}
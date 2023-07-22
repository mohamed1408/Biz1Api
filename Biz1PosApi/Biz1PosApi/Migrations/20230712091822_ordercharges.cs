using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class ordercharges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCharges_AdditionalCharges_AdditionalChargeId",
                table: "OrderCharges");

            migrationBuilder.AlterColumn<int>(
                name: "AdditionalChargeId",
                table: "OrderCharges",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCharges_AdditionalCharges_AdditionalChargeId",
                table: "OrderCharges",
                column: "AdditionalChargeId",
                principalTable: "AdditionalCharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderCharges_AdditionalCharges_AdditionalChargeId",
                table: "OrderCharges");

            migrationBuilder.AlterColumn<int>(
                name: "AdditionalChargeId",
                table: "OrderCharges",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCharges_AdditionalCharges_AdditionalChargeId",
                table: "OrderCharges",
                column: "AdditionalChargeId",
                principalTable: "AdditionalCharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

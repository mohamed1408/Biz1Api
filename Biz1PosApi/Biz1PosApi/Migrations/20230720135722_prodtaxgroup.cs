using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class prodtaxgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TaxGroups_TaxGroupId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "TaxGroupId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TaxGroups_TaxGroupId",
                table: "Products",
                column: "TaxGroupId",
                principalTable: "TaxGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_TaxGroups_TaxGroupId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "TaxGroupId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TaxGroups_TaxGroupId",
                table: "Products",
                column: "TaxGroupId",
                principalTable: "TaxGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

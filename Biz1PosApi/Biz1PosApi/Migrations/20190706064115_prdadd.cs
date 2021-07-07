using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class prdadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAddOns_Products_AddOnId",
                table: "ProductAddOns");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAddOns_Addons_AddOnId",
                table: "ProductAddOns",
                column: "AddOnId",
                principalTable: "Addons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAddOns_Addons_AddOnId",
                table: "ProductAddOns");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAddOns_Products_AddOnId",
                table: "ProductAddOns",
                column: "AddOnId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class prdad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAddOns_AddonGroups_AddOnGrpId",
                table: "ProductAddOns");

            migrationBuilder.DropIndex(
                name: "IX_ProductAddOns_AddOnGrpId",
                table: "ProductAddOns");

            migrationBuilder.DropColumn(
                name: "AddOnGrpId",
                table: "ProductAddOns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddOnGrpId",
                table: "ProductAddOns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAddOns_AddOnGrpId",
                table: "ProductAddOns",
                column: "AddOnGrpId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAddOns_AddonGroups_AddOnGrpId",
                table: "ProductAddOns",
                column: "AddOnGrpId",
                principalTable: "AddonGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

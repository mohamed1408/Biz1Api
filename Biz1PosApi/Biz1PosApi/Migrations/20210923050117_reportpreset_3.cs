using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class reportpreset_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportPresets_Stores_StoreId",
                table: "ReportPresets");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ReportPresets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPresets_Stores_StoreId",
                table: "ReportPresets",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportPresets_Stores_StoreId",
                table: "ReportPresets");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ReportPresets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportPresets_Stores_StoreId",
                table: "ReportPresets",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

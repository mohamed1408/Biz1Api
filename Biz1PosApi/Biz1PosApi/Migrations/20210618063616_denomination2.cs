using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class denomination2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Denominations_DenomEntries_DenomEntryId",
                table: "Denominations");

            migrationBuilder.AlterColumn<int>(
                name: "DenomEntryId",
                table: "Denominations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Denominations_DenomEntries_DenomEntryId",
                table: "Denominations",
                column: "DenomEntryId",
                principalTable: "DenomEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Denominations_DenomEntries_DenomEntryId",
                table: "Denominations");

            migrationBuilder.AlterColumn<int>(
                name: "DenomEntryId",
                table: "Denominations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Denominations_DenomEntries_DenomEntryId",
                table: "Denominations",
                column: "DenomEntryId",
                principalTable: "DenomEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

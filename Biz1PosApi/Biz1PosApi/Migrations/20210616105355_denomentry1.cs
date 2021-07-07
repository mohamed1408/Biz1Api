using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class denomentry1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditedForId",
                table: "DenomEntries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DenomEntries_EditedForId",
                table: "DenomEntries",
                column: "EditedForId");

            migrationBuilder.AddForeignKey(
                name: "FK_DenomEntries_DenomEntries_EditedForId",
                table: "DenomEntries",
                column: "EditedForId",
                principalTable: "DenomEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DenomEntries_DenomEntries_EditedForId",
                table: "DenomEntries");

            migrationBuilder.DropIndex(
                name: "IX_DenomEntries_EditedForId",
                table: "DenomEntries");

            migrationBuilder.DropColumn(
                name: "EditedForId",
                table: "DenomEntries");
        }
    }
}

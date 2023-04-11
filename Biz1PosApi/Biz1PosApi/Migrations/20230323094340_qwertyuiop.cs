using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qwertyuiop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KOTInstructions_KOTs_KOTId",
                table: "KOTInstructions");

            migrationBuilder.RenameColumn(
                name: "KOTId",
                table: "KOTInstructions",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_KOTInstructions_KOTId",
                table: "KOTInstructions",
                newName: "IX_KOTInstructions_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_KOTInstructions_Orders_OrderId",
                table: "KOTInstructions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KOTInstructions_Orders_OrderId",
                table: "KOTInstructions");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "KOTInstructions",
                newName: "KOTId");

            migrationBuilder.RenameIndex(
                name: "IX_KOTInstructions_OrderId",
                table: "KOTInstructions",
                newName: "IX_KOTInstructions_KOTId");

            migrationBuilder.AddForeignKey(
                name: "FK_KOTInstructions_KOTs_KOTId",
                table: "KOTInstructions",
                column: "KOTId",
                principalTable: "KOTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

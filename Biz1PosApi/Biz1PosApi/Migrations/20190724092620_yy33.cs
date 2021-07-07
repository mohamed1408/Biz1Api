using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class yy33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "KOTId",
                table: "OrderItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems",
                column: "KOTId",
                principalTable: "KOTs",
                principalColumn: "Id");
                //onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "KOTId",
                table: "OrderItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_KOTs_KOTId",
                table: "OrderItems",
                column: "KOTId",
                principalTable: "KOTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

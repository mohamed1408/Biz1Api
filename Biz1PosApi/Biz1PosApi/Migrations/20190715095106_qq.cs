using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypes_OrdTypeId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrdTypeId",
                table: "Orders",
                newName: "OrderTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OrdTypeId",
                table: "Orders",
                newName: "IX_Orders_OrderTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId",
                principalTable: "OrderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderTypeId",
                table: "Orders",
                newName: "OrdTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders",
                newName: "IX_Orders_OrdTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypes_OrdTypeId",
                table: "Orders",
                column: "OrdTypeId",
                principalTable: "OrderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

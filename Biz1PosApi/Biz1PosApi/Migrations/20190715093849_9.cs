using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdTypeId",
                table: "Orders",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrdTypeId",
                table: "Orders",
                column: "OrdTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypes_OrdTypeId",
                table: "Orders",
                column: "OrdTypeId",
                principalTable: "OrderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypes_OrdTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrdTypeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrdTypeId",
                table: "Orders");
        }
    }
}

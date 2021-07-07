using Microsoft.EntityFrameworkCore.Migrations;

namespace Biz1PosApi.Migrations
{
    public partial class qwertyuiopasdfghjkl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryStoreId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryStoreId",
                table: "Orders",
                column: "DeliveryStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stores_DeliveryStoreId",
                table: "Orders",
                column: "DeliveryStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stores_DeliveryStoreId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryStoreId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryStoreId",
                table: "Orders");
        }
    }
}
